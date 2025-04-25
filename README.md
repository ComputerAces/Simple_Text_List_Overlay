# Simple Text List Overlay

A lightweight Windows application that displays lines of text from a user-specified file as a customizable, transparent overlay on your screen. Ideal for showing pro tips, reminders, quotes, or any list of text items cyclically.

Built with C# WinForms targeting .NET Framework 4.7.2, utilizing P/Invoke calls to the Win32 API (`UpdateLayeredWindow`) for true per-pixel alpha transparency.

## Features

* **Text File Source:** Reads text items from a standard `.txt` file (one item per line).
* **Ignores Blank Lines:** Automatically filters out empty or whitespace-only lines from the source file.
* **Customizable Display:**
    * Selectable Font Family, Size, and Style.
    * Customizable Text Color.
    * Optional Text Outline with customizable Color.
* **Cycling Options:**
    * Adjustable display time per item (in seconds).
    * Choose between Sequential or Random order for displaying items.
* **True Transparency:** Uses Layered Windows API for a fully transparent background, showing only the text/outline over your desktop content.
* **Auto-Sizing:** Automatically adjusts overlay height based on the current text item's content and selected font size.
* **System Tray Control:** Managed via a Notification Area (System Tray) icon:
    * Access to a comprehensive **Settings** dialog.
    * Option to **Show/Hide** the overlay window.
    * Manually trigger the **Next Tip**.
    * Toggle **Move/Resize** mode (temporarily adds borders for easy repositioning and resizing).
    * **Exit** the application.
* **Persistent Settings:** Remembers your chosen text file, appearance settings, timing, order preference, and window position/size between sessions.

## Screenshots

*(It's highly recommended to add screenshots here!)*

* *(Example: Screenshot of the overlay displaying a tip)*
* *(Example: Screenshot of the Settings window)*

## Requirements

* **Operating System:** Windows
* **.NET Framework:** Microsoft .NET Framework 4.7.2 Runtime (Download from Microsoft if not already installed).

## Installation & Usage

1.  **Download:** Grab the latest executable (`Simple_Text_List_Overlay.exe`) from the [Releases page](https://github.com/YourUsername/YourRepoName/releases) *(<- Update this link!)* on GitHub.
2.  **Run:** Place the executable in a folder of your choice and run it.
3.  **Configure:**
    * The application starts minimized to the System Tray (Notification Area). Find its icon.
    * **Right-click** the tray icon and select **Settings...**.
    * In the Settings window, click **Browse...** and select a `.txt` file containing the text items you want to display (one item per line).
    * Adjust the Font, Colors, Outline, Display Time, and Random Order options as desired.
    * Click **Save**.
4.  **Use:** The overlay should appear and start cycling through your text file. Right-click the tray icon to access options like "Next Tip", "Show/Hide Overlay", "Move/Resize", or to exit.

## Building from Source

1.  **Clone:** Clone this repository to your local machine.
2.  **IDE:** Open the `.sln` solution file in Visual Studio 2019 or a later version that supports .NET Framework 4.7.2.
3.  **Framework:** Ensure you have the .NET Framework 4.7.2 Targeting Pack installed (via Visual Studio Installer -> Individual components).
4.  **Icon:** The project requires an icon file (`.ico`) to be assigned to the `trayIcon` component in the `CoreForm.cs` designer. You may need to provide your own `.ico` file if the placeholder is missing or not suitable.
5.  **Build:** Build the solution (`Build` -> `Build Solution` or `F6`). The executable will be in the `bin\Debug` or `bin\Release` folder.

## Configuration Storage

Application settings (file path, appearance, position, etc.) are stored in a `user.config` file located within your user profile's application data folder (typically `%LOCALAPPDATA%\Simple_Text_List_Overlay...`).

## License

*(Choose a license! MIT is a common choice for open source.)*

This project is licensed under the MIT License - see the `LICENSE` file for details. *(You'll need to add a LICENSE file containing the actual license text, e.g., the standard MIT license text)*.