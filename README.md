How it works:
After application starts it prompts you to imput your credentials (user name and password) and confirm your password. Password and user name is encrypted with aes256 and stored in program variables. Saving to external storage was not implemented.
Then main window is opened where you can click on "Discover" button for searching all bluetooth devices in range.
Once they are found and populated into drop-down list you can choose any of them and click button "Start using selected device for security".
From this moment the program checks in background if the selected device is still in range. 
If device is out of range - program is locked and shows a dialog for authentication. 
When user inputs correct credentials and device is already in range - program can be unlocked. Otherwise it stay locked.

Tools:
  IDE: Visual Studio 2013;
  Plarform: .net framework 4.5;
  Language: C#;
  Bluetooth library: 32feet.net;
