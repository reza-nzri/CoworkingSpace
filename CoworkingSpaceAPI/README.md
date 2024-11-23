# 🚀 CoworkingSpaceAPI

## 📦 Install Dependencies (NuGet Packages)

### For Windows (Batch Script)

To install all dependencies for the project, follow these steps:

1. Run the provided `install-packages.bat` script by double-clicking it or executing the following command in the terminal:

   ```bash
   install-packages.bat
   ```

### For Linux/MacOS (Shell Script)

1. Convert the `install-packages.bat` script to a shell script:
   - Copy the contents of `install-packages.bat` into a new file.
   - Add the following line at the top of the file to specify the interpreter:

     ```bash
     #!/bin/bash
     ```

   - Save the file with a `.sh` extension, for example: `install-packages.sh`.

2. Make the script executable by running:

   ```bash
   chmod +x install-packages.sh
   ```

3. Execute the script to install all required dependencies:

   ```bash
   ./install-packages.sh
   ```

## 🔧 Commands

```bash
# To restore all NuGet packages required for the project
dotnet restore
```

## ✅ Enjoy Developing

Use these tools and commands to quickly set up and manage dependencies for the **CoworkingSpaceAPI**. Happy coding! 😊
