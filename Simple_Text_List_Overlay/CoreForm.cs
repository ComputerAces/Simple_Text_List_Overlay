using System;
using System.Collections.Generic;
using System.ComponentModel; // For TypeConverter attribute (if needed)
using System.Drawing;
using System.Drawing.Drawing2D; // For GraphicsPath, SmoothingMode
using System.Drawing.Imaging;   // For PixelFormat
using System.Drawing.Text;      // For TextRenderingHint
using System.IO;
using System.Linq;
using System.Runtime.InteropServices; // For DllImport
using System.Windows.Forms;
using System.Diagnostics;

// Ensure this namespace matches your project name
namespace Simple_Text_List_Overlay {
    public partial class CoreForm : Form {
        // --- Fields ---
        private List<string> tips = new List<string>();
        private int currentTipIndex = -1;
        private string currentTipText = "Loading...";
        private Timer tipTimer;

        // Settings Cache
        private Font currentFont = new Font("Arial Black", 16, FontStyle.Bold); // Default fallback font
        private Color currentTextColor = Color.White;
        private Color currentOutlineColor = Color.Black;
        private bool useOutline = true;
        private string tipsFilePath = "";
        private int switchTimeMs = 30000; // Default 30 seconds

        // State Flags
        private bool isMovingResizing = false;
        private bool isOverlayVisible = true;

        private bool randomOrder = false; // Cache for the setting
        private Random randomGen = new Random(); // Random number generator


        public CoreForm()
        {
            InitializeComponent(); // From CoreForm.Designer.cs

            // We set properties like FormBorderStyle, TopMost etc. based on mode
            // Initial setup happens in Load event after settings are loaded
        }

        // --- Overrides for Layered Window & Dragging ---

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Add the WS_EX_LAYERED extended style ONLY when not in move/resize mode
                if (!isMovingResizing)
                {
                    cp.ExStyle |= NativeMethods.WS_EX_LAYERED;
                }
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;

            // Allow dragging via client area ONLY when FormBorderStyle is None (overlay mode)
            if (m.Msg == WM_NCHITTEST && this.FormBorderStyle == FormBorderStyle.None)
            {
                base.WndProc(ref m);
                if (m.Result.ToInt32() == HTCLIENT)
                {
                    m.Result = (IntPtr)HTCAPTION; // Treat client area as caption bar for dragging
                }
                return;
            }

            base.WndProc(ref m); // Process other messages normally
        }


        // --- Form Load / Closing ---

        private void CoreForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadTipsFromFile();

            tipTimer = new Timer();
            tipTimer.Interval = switchTimeMs;
            tipTimer.Tick += TipTimer_Tick;

            this.Size = Properties.Settings.Default.WindowSize;
            this.Location = Properties.Settings.Default.WindowLocation;
            EnsureFormIsOnScreen();

            // --- Explicitly set properties for Layered Mode ---
            this.Opacity = 1.0;      // Standard opacity full (transparency via bitmap alpha)
            this.BackColor = Color.Black; // Background color (shouldn't be visible)
            // -------------------------------------------------

            if (tips.Count > 0)
            {
                if (randomOrder && tips.Count > 1)
                {
                    currentTipIndex = randomGen.Next(0, tips.Count);
                    currentTipText = tips[currentTipIndex];
                }
                else
                {
                    currentTipIndex = 0;
                    currentTipText = tips[0];
                }
                tipTimer.Start();
            }

            UpdateFormDisplay();
            trayIcon.Visible = true;
        }

        private void CoreForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up resources
            tipTimer?.Stop();
            tipTimer?.Dispose();
            currentFont?.Dispose();

            // Hide tray icon BEFORE closing to avoid ghost icon
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }
        }

        // --- Settings Load/Save ---

        private void LoadSettings()
        {
            // Load values, providing defaults if settings are new/invalid
            Properties.Settings settings = Properties.Settings.Default;

            tipsFilePath = settings.TipsFilePath;
            useOutline = settings.UseOutline;
            currentOutlineColor = settings.OutlineColor;
            currentTextColor = settings.TextColor;
            randomOrder = settings.RandomOrder; // Load the RandomOrder setting

            // Safety check: If loaded color is somehow invalid/empty, use fallback
            if (currentOutlineColor.IsEmpty) currentOutlineColor = Color.Black;
            if (currentTextColor.IsEmpty) currentTextColor = Color.White;

            switchTimeMs = Math.Max(1000, settings.SwitchTimeSeconds * 1000); // Ensure at least 1 sec interval

            try
            {
                currentFont?.Dispose();
                currentFont = settings.TextFont;
                if (currentFont == null)
                {
                    currentFont = new Font("Arial Black", 16, FontStyle.Bold); // Fallback
                }
            }
            catch (Exception ex) // Catch potential errors during Font deserialization
            {
                Console.WriteLine($"Error loading font setting: {ex.Message}. Using fallback."); // Keep Console for errors
                currentFont?.Dispose();
                currentFont = new Font("Arial Black", 16, FontStyle.Bold); // Fallback
            }

            // Ensure Random object is ready
            if (randomGen == null)
            {
                randomGen = new Random();
            }
            // Location and Size are loaded directly in Form_Load
        }

        private void SaveFormPositionAndSize()
        {
            Properties.Settings settings = Properties.Settings.Default;
            settings.WindowLocation = this.Location;
            settings.WindowSize = this.Size;
            settings.Save();
        }

        // --- Tip Loading ---

        private void LoadTipsFromFile()
        {
            tips.Clear(); // Clear existing tips before loading
            string statusMessage = "Loading..."; // Default status

            if (!string.IsNullOrEmpty(tipsFilePath) && File.Exists(tipsFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(tipsFilePath);
                    // Filter out blank lines
                    // Make sure 'using System.Linq;' is at the top of CoreForm.cs
                    tips.AddRange(lines.Where(line => !string.IsNullOrWhiteSpace(line)));

                    if (tips.Count == 0)
                    {
                        statusMessage = "Tips file is empty or contains only blank lines.";
                    }
                    // If tips were loaded, statusMessage will be replaced by the first tip below
                }
                catch (Exception ex) // Catch more specific exceptions if needed
                {
                    statusMessage = $"Error reading tips file: {ex.ShortMessage()}";
                    Console.WriteLine($"Error loading tips file '{tipsFilePath}': {ex}"); // Keep Console for errors
                }
            }
            else if (string.IsNullOrEmpty(tipsFilePath))
            {
                statusMessage = "Tips file path not set in Settings.";
            }
            else // File path is set but file doesn't exist
            {
                statusMessage = $"Tips file not found: {tipsFilePath}";
            }

            // Reset index and set currentTipText based on load result
            currentTipIndex = -1;
            if (tips.Count > 0)
            {
                currentTipIndex = 0;
                currentTipText = tips[0]; // Set text to the first valid tip
            }
            else
            {
                currentTipText = statusMessage; // Set text to the status/error message
            }
        }


        // --- Timer ---

        private void TipTimer_Tick(object sender, EventArgs e)
        {
            if (tips != null && tips.Count > 0 && isOverlayVisible) // Also check visibility
            {
                int previousTipIndex = currentTipIndex; // Store previous index for comparison

                if (randomOrder)
                {
                    // --- Random Order Logic ---
                    if (tips.Count == 1)
                    {
                        currentTipIndex = 0; // Only one tip, index must be 0
                    }
                    else
                    {
                        // Generate a new random index until it's different from the current one
                        int newIndex;
                        int attempt = 0; // Prevent potential infinite loop
                        do
                        {
                            newIndex = randomGen.Next(0, tips.Count);
                            attempt++;
                        } while (newIndex == previousTipIndex && attempt < 10); // Avoid immediate repeat, limit attempts
                        currentTipIndex = newIndex;
                    }
                }
                else
                {
                    // --- Sequential Order Logic ---
                    currentTipIndex = (previousTipIndex + 1) % tips.Count; // Wrap index
                }

                // Update the text and display
                currentTipText = tips[currentTipIndex];
                UpdateFormDisplay();
            }
        }

        // --- Drawing Logic ---

        private Bitmap PaintContent(string textToDraw, out Size requiredSize)
        {
            // Sensible defaults in case of error
            requiredSize = this.ClientSize; // Start with current size
            if (requiredSize.Width <= 0) requiredSize.Width = 100;
            if (requiredSize.Height <= 0) requiredSize.Height = 20;

            // Measure text
            SizeF measuredSize;
            // Use a temporary bitmap for measuring
            using (Bitmap measureBitmap = new Bitmap(1, 1))
            using (Graphics measurer = Graphics.FromImage(measureBitmap))
            {
                float availableWidth = requiredSize.Width - 20; // 10px padding each side
                if (availableWidth <= 0) availableWidth = 1;
                // Assume currentFont is valid here (loaded in LoadSettings with fallback)
                measuredSize = measurer.MeasureString(textToDraw, currentFont, (int)availableWidth);
            }

            // Calculate required size (add padding)
            int padding = 10;
            requiredSize = new Size(requiredSize.Width, (int)Math.Ceiling(measuredSize.Height) + (padding * 2));
            if (requiredSize.Height <= 0) requiredSize.Height = 20;


            // Create bitmap with CALCULATED size and Alpha channel support
            Bitmap bitmap = new Bitmap(requiredSize.Width, requiredSize.Height, PixelFormat.Format32bppArgb);

            try
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    // --- Clear the bitmap background with fully transparent color ---
                    g.Clear(Color.FromArgb(0, 0, 0, 0));
                    // ---------------------------------------------------------------

                    RectangleF textRect = new RectangleF(padding, padding, requiredSize.Width - (padding * 2), requiredSize.Height - (padding * 2));

                    // Draw using GraphicsPath for outline
                    using (GraphicsPath path = new GraphicsPath())
                    using (StringFormat format = new StringFormat())
                    {
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;

                        // Assume currentFont is valid
                        path.AddString(textToDraw, currentFont.FontFamily, (int)currentFont.Style,
                                       g.DpiY * currentFont.Size / 72f, // Convert point size to pixels
                                       textRect, format);

                        // Draw outline if enabled
                        if (useOutline)
                        {
                            // Assume currentOutlineColor is valid
                            using (Pen outlinePen = new Pen(currentOutlineColor, 2)) // Adjust thickness as needed
                            {
                                outlinePen.LineJoin = LineJoin.Round;
                                g.DrawPath(outlinePen, path);
                            }
                        }

                        // Fill text
                        // Assume currentTextColor is valid
                        using (Brush fillBrush = new SolidBrush(currentTextColor))
                        {
                            g.FillPath(fillBrush, path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error painting content: " + ex.Message);
                // Handle error - draw error message onto bitmap
                using (Graphics gErr = Graphics.FromImage(bitmap))
                {
                    gErr.Clear(Color.FromArgb(150, 255, 0, 0));
                    using (Font errFont = new Font("Arial", 10))
                    { // Ensure errFont is disposed
                        gErr.DrawString("Paint Error", errFont, Brushes.White, 5, 5);
                    }
                }
            }
            return bitmap;
        }

        private void UpdateFormDisplay()
        {
            // Don't update if in move/resize mode, handle not created, form not visible,
            // or if disposed (added check for safety)
            if (isMovingResizing || !IsHandleCreated || !this.Visible || this.IsDisposed)
            {
                return;
            }

            Bitmap bitmap = null;
            IntPtr screenDc = IntPtr.Zero;
            IntPtr memDc = IntPtr.Zero;
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;
            Size newSize = ClientSize; // Start with current size

            try
            {
                // --- Paint Content ---
                // Pass current ClientSize as a hint for width measurement
                bitmap = PaintContent(currentTipText, out newSize);
                if (bitmap == null)
                {
                    // Handle case where PaintContent fails unexpectedly
                    Console.WriteLine("Error: PaintContent returned null.");
                    return;
                }


                // --- Adjust form size if needed before updating ---
                // Check dimensions are valid and different from current ClientSize
                if (newSize.Width > 0 && newSize.Height > 0 && this.ClientSize != newSize)
                {
                    // Check again if handle exists before setting size
                    if (IsHandleCreated && !this.IsDisposed)
                    {
                        this.ClientSize = newSize;
                    }
                    else
                    {
                        // Cannot resize if handle isn't ready or form is gone
                        return;
                    }
                }
                // Update size variable to match bitmap size for UpdateLayeredWindow
                newSize = new Size(bitmap.Width, bitmap.Height);


                // --- GDI Handle Preparation ---
                // Use Color.FromArgb(0) for transparent background key when getting Hbitmap
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0)); // Specify background color for handle creation (important for alpha)
                if (hBitmap == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "GetHbitmap returned IntPtr.Zero");
                }

                screenDc = NativeMethods.GetDC(IntPtr.Zero); // Get DC for entire screen
                if (screenDc == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "GetDC(IntPtr.Zero) failed");
                }

                memDc = NativeMethods.CreateCompatibleDC(screenDc); // Create DC compatible with screen
                if (memDc == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateCompatibleDC failed");
                }

                // Select the new bitmap into the memory DC, keeping track of the old one
                // IMPORTANT: SelectObject returns the *previously* selected object.
                oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);
                if (oldBitmap == IntPtr.Zero)
                {
                    // Log error, but might not be fatal depending on context
                    Console.WriteLine("Warning: SelectObject returned IntPtr.Zero for oldBitmap.");
                }


                // --- Setup for UpdateLayeredWindow ---
                // Use actual bitmap dimensions
                NativeMethods.SIZE size = new NativeMethods.SIZE(bitmap.Width, bitmap.Height);
                NativeMethods.POINT pointSource = new NativeMethods.POINT(0, 0); // Top-left corner of source bitmap
                NativeMethods.POINT topPos = new NativeMethods.POINT(this.Left, this.Top); // Top-left corner of window on screen

                NativeMethods.BLENDFUNCTION blend = new NativeMethods.BLENDFUNCTION();
                blend.BlendOp = NativeMethods.AC_SRC_OVER;      // Standard alpha blending operation
                blend.BlendFlags = 0;                           // Must be 0 for AC_SRC_OVER
                blend.SourceConstantAlpha = 255;                // Use 100% of source bitmap's alpha (per-pixel)
                blend.AlphaFormat = NativeMethods.AC_SRC_ALPHA; // Indicate source bitmap has an alpha channel


                // --- Update the layered window ---
                // Ensure handle is still valid before calling
                if (IsHandleCreated && !this.IsDisposed)
                {
                    bool success = NativeMethods.UpdateLayeredWindow(
                        this.Handle,    // Window handle
                        screenDc,       // DC for screen (null implies screen)
                        ref topPos,     // Screen position of window
                        ref size,       // Size of bitmap (and window area to update)
                        memDc,          // Source DC holding the bitmap
                        ref pointSource,// Top-left position in source DC (usually 0,0)
                        0,              // Color key (not used with ULW_ALPHA)
                        ref blend,      // Alpha blending function
                        NativeMethods.ULW_ALPHA // Use per-pixel alpha from the source bitmap
                    );

                    if (!success)
                    {
                        // Throw detailed exception if the critical function fails
                        throw new Win32Exception(Marshal.GetLastWin32Error(), $"UpdateLayeredWindow failed. Location: {this.Location}, Size: {this.Size}, Bitmap Size: {bitmap?.Size}");
                    }
                }

            }
            catch (Win32Exception winEx)
            {
                Console.WriteLine($"Win32 Error in UpdateFormDisplay: {winEx.Message} (Code: {winEx.NativeErrorCode})");
                // Optionally, display a user-friendly error or fallback state
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in UpdateFormDisplay: {ex.Message}");
                // Optionally, display a user-friendly error or fallback state
            }
            finally
            {
                // --- GDI Resource Cleanup ---
                // ALWAYS release/delete handles in reverse order of creation/selection

                // Release the screen DC
                if (screenDc != IntPtr.Zero)
                {
                    NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
                    screenDc = IntPtr.Zero; // Prevent double release
                }

                // Deselect the bitmap from the memory DC *if* an old bitmap handle exists
                // (Selecting the old bitmap back is the proper way before deleting the memDc)
                // However, SelectObject can return null or a default stock object handle (like 1)
                // which should not be deleted. Deleting memDc implicitly deselects the current object.
                // So, we only need to delete the hBitmap we created.

                // Delete the memory DC
                if (memDc != IntPtr.Zero)
                {
                    NativeMethods.DeleteDC(memDc);
                    memDc = IntPtr.Zero; // Prevent double deletion
                }

                // Delete the GDI bitmap handle we created from the managed Bitmap
                // Do NOT delete oldBitmap unless you are certain it's a handle you created and must manage.
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.DeleteObject(hBitmap);
                    hBitmap = IntPtr.Zero; // Prevent double deletion
                }

                // Dispose the managed Bitmap object
                bitmap?.Dispose();
            }
        }

        // --- Menu Item Handlers ---

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            tipTimer.Stop();

            using (SettingsForm settingsForm = new SettingsForm())
            {
                DialogResult result = settingsForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    // Reload settings into the fields/variables
                    LoadSettings();
                    // Reload tips from potentially new file path
                    LoadTipsFromFile();
                    // Update timer interval from reloaded settings
                    tipTimer.Interval = switchTimeMs;
                    // Force immediate update of display with new settings/tip
                    UpdateFormDisplay();
                }
            }
            // Restart timer only if tips exist and form is visible
            if (tips != null && tips.Count > 0 && isOverlayVisible)
            {
                tipTimer.Start();
            }
        }

        private void menuItemShowHide_Click(object sender, EventArgs e)
        {
            isOverlayVisible = !isOverlayVisible;
            this.Visible = isOverlayVisible;
            menuItemShowHide.Text = isOverlayVisible ? "Hide Overlay" : "Show Overlay";
            // If showing, trigger an update
            if (isOverlayVisible)
            {
                UpdateFormDisplay();
            }
        }

        private void menuItemMoveResize_Click(object sender, EventArgs e)
        {
            isMovingResizing = !isMovingResizing;

            if (isMovingResizing)
            {
                // --- Enter Move/Resize Mode ---
                tipTimer.Stop();
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.BackColor = SystemColors.Control; // Standard background
                this.Opacity = 1.0;                   // Standard opaque
                RecreateHandle();
                menuItemMoveResize.Text = "Done Moving/Resizing";
                this.Controls.Clear();
                Label moveLabel = new Label { Text = "Move or resize this window.\nClick tray icon -> 'Done Moving/Resizing' when finished.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
                this.Controls.Add(moveLabel);
                this.Invalidate();
            }
            else
            {
                // --- Exit Move/Resize Mode ---
                this.Controls.Clear();
                SaveFormPositionAndSize();

                // --- Explicitly set properties for Layered Mode BEFORE RecreateHandle ---
                this.Opacity = 1.0;      // Standard opacity full
                this.BackColor = Color.Black; // Background color (should be irrelevant)
                // --------------------------------------------------------------------

                // Set state BEFORE recreating handle
                this.FormBorderStyle = FormBorderStyle.None;

                RecreateHandle(); // CreateParams should run here and apply WS_EX_LAYERED

                menuItemMoveResize.Text = "Move/Resize";

                // Ensure text is valid
                if (tips == null || tips.Count == 0) { currentTipIndex = -1; }
                else { currentTipIndex = Math.Max(0, Math.Min(currentTipIndex, tips.Count - 1)); currentTipText = tips[currentTipIndex]; }

                // Force Update using BeginInvoke
                this.BeginInvoke((MethodInvoker)delegate {
                    UpdateFormDisplay();
                });

                // Restart timer if tips exist
                if (tips != null && tips.Count > 0 && isOverlayVisible)
                {
                    tipTimer.Start();
                }
            }
        }

        private void menuItemNextTip_Click(object sender, EventArgs e)
        {
            // Check if there are tips to cycle through and if the overlay is visible
            if (tips != null && tips.Count > 0 && isOverlayVisible)
            {
                tipTimer.Stop(); // Stop timer FIRST

                int previousTipIndex = currentTipIndex;
                int newIndex = previousTipIndex; // Initialize

                if (randomOrder)
                {
                    // --- Random Order Logic ---
                    if (tips.Count == 1) { newIndex = 0; }
                    else
                    {
                        int attempt = 0;
                        do
                        {
                            newIndex = randomGen.Next(0, tips.Count);
                            attempt++;
                        } while (newIndex == previousTipIndex && attempt < 10); // Avoid immediate repeat
                    }
                }
                else
                {
                    // --- Sequential Order Logic ---
                    newIndex = (previousTipIndex + 1) % tips.Count; // Wrap index
                }

                // --- Update State ---
                currentTipIndex = newIndex;
                currentTipText = tips[currentTipIndex];

                // --- Update Display and Restart Timer ---
                UpdateFormDisplay();
                tipTimer.Start(); // Restart timer AFTER updating display
            }
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Close the application
        }

        // --- Utility ---
        private void EnsureFormIsOnScreen()
        {
            Screen screen = Screen.FromPoint(this.Location); // Find screen form is on (or nearest)
            Rectangle screenRect = screen.WorkingArea;

            // Check if top-left corner is off-screen
            if (this.Left < screenRect.Left) this.Left = screenRect.Left;
            if (this.Top < screenRect.Top) this.Top = screenRect.Top;

            // Check if bottom-right corner is off-screen
            if (this.Right > screenRect.Right) this.Left = screenRect.Right - this.Width;
            if (this.Bottom > screenRect.Bottom) this.Top = screenRect.Bottom - this.Height;

            // Secondary check for top-left again after adjusting right/bottom
            if (this.Left < screenRect.Left) this.Left = screenRect.Left;
            if (this.Top < screenRect.Top) this.Top = screenRect.Top;

            // Ensure minimum size just in case saved size was tiny
            if (this.Width < 50) this.Width = 50;
            if (this.Height < 20) this.Height = 20;
        }
    }

    // --- P/Invoke Declarations ---
    // (Keep the NativeMethods class exactly as provided in the previous step)
    internal static class NativeMethods {
        public const int WS_EX_LAYERED = 0x80000;
        // Removed GWL_EXSTYLE as CreateParams is used instead
        // public const int GWL_EXSTYLE = -20;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
        public const int ULW_ALPHA = 0x02;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X; public int Y; public POINT(int x, int y) { this.X = x; this.Y = y; } }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE { public int cx; public int cy; public SIZE(int x, int y) { this.cx = x; this.cy = y; } }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION { public byte BlendOp; public byte BlendFlags; public byte SourceConstantAlpha; public byte AlphaFormat; }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pptSrc, uint crKey, ref BLENDFUNCTION pblend, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);
    }

    // --- Add this helper method for shorter error messages ---
    public static class ExceptionExtensions {
        public static string ShortMessage(this Exception ex)
        {
            if (ex == null) return string.Empty;
            // Return only the first line of the exception message
            var lines = ex.Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Length > 0 ? lines[0] : "An unspecified error occurred.";
        }
    }
}