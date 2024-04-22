    using System;
    using System.IO;
    using System.Text;

    namespace SubtitleEncoder;

    public static class EncodeLogic
    {
        public static async Task EncodeIntoUtf8(string inputPathFolder, string outputPathFolder, bool saveInFixedFolder)
        {
            // Register the code page provider for Windows-1256 encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Check if the input folder exists
            if (Directory.Exists(inputPathFolder))
            {
                // Process files in the current folder
                await ProcessFilesInFolder(inputPathFolder, outputPathFolder, saveInFixedFolder);

                // Process subfolders recursively
                string[] subfolders = Directory.GetDirectories(inputPathFolder);
                foreach (string subfolder in subfolders)
                {
                    // Recursively call EncodeIntoUtf8 for each subfolder
                    await EncodeIntoUtf8(subfolder, outputPathFolder, saveInFixedFolder);
                }
            }
            else
            {
                Console.WriteLine("Input folder does not exist.");
            }
        }


        static async Task ProcessFilesInFolder(string inputFolder, string outputFolder, bool saveInFixedFolder)
    {
        // Ensure the output folder exists, if not, create it
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        // Get all files in the input folder
        string[] inputFiles = Directory.GetFiles(inputFolder);

        // Process each file
        foreach (string inputFile in inputFiles)
        {
            // Check if the file has the desired extension
            string extension = Path.GetExtension(inputFile);
            if (extension != ".ass" && extension != ".srt")
            {
                Console.WriteLine($"Skipping file {Path.GetFileName(inputFile)} as it is not in .ass or .srt format.");
                continue; // Skip to the next file
            }

            // Determine the output folder for this file based on the saving option
            string fileOutputFolder = saveInFixedFolder ? outputFolder : Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(inputFolder));

            // Ensure the output subfolder exists, if not, create it
            if (!Directory.Exists(fileOutputFolder))
            {
                Directory.CreateDirectory(fileOutputFolder);
            }

            // Detect the encoding of the input file
            Encoding encoding = DetectEncoding(inputFile);

            // Output the detected encoding of the input file
            if (encoding != null)
            {
                Console.WriteLine($"The encoding of {Path.GetFileName(inputFile)} is: {encoding.EncodingName}");
            }
            else
            {
                Console.WriteLine($"Failed to detect the encoding of {Path.GetFileName(inputFile)}.");
                continue; // Skip to the next file
            }

            // Specify the output file path
            string outputFile = Path.Combine(fileOutputFolder, Path.GetFileName(inputFile));

            // If the encoding is not UTF-8, perform conversion
            if (encoding != Encoding.UTF8)
            {
                // Read the input file using the Arabic(Windows) encoding
                string content = File.ReadAllText(inputFile, Encoding.GetEncoding(1256));

                // Write the content back to the output file using UTF-8 encoding
                await File.WriteAllTextAsync(outputFile, content, Encoding.UTF8);

                Console.WriteLine($"{Path.GetFileName(inputFile)} converted successfully with name: {inputFile} .");
            }
            else
            {
                // If the encoding is UTF-8, asynchronously copy the file to the output folder
                await CopyFileAsync(inputFile, outputFile);
                Console.WriteLine($"{Path.GetFileName(inputFile)} copied to output folder.");
            }
        }
    }


        static async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read,
                       bufferSize: 4096, useAsync: true))
            using (FileStream destinationStream = new FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write,
                       FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await sourceStream.CopyToAsync(destinationStream);
            }
        }

        static Encoding DetectEncoding(string filePath)
        {
            // Read the first few bytes of the file to determine the encoding
            byte[] buffer = new byte[1024];
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                // Attempt to detect Windows-1256 encoding based on content
                if (ContainsWindows1256Content(buffer, bytesRead))
                {
                    return Encoding.GetEncoding(1256); // Windows-1256
                }
                // Check for UTF-8 BOM (EF BB BF)
                else if (bytesRead >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                {
                    return Encoding.UTF8;
                }
                // Check for UTF-16 (LE) BOM (FF FE)
                else if (bytesRead >= 2 && buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return Encoding.Unicode; // UTF-16LE
                }
                // Check for UTF-16 (BE) BOM (FE FF)
                else if (bytesRead >= 2 && buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return Encoding.BigEndianUnicode; // UTF-16BE
                }
                // Check for UTF-32 (LE) BOM (FF FE 00 00)
                else if (bytesRead >= 4 && buffer[0] == 0xFF && buffer[1] == 0xFE && buffer[2] == 0x00 &&
                         buffer[3] == 0x00)
                {
                    return Encoding.UTF32; // UTF-32LE
                }
                // Check for UTF-32 (BE) BOM (00 00 FE FF)
                else if (bytesRead >= 4 && buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0xFE &&
                         buffer[3] == 0xFF)
                {
                    return new UTF32Encoding(true, true); // UTF-32BE
                }
                else
                {
                    // If BOM is not found and content doesn't match known encodings, return null
                    return null;
                }
            }
        }

        static bool ContainsWindows1256Content(byte[] buffer, int length)
        {
            // Windows-1256 encoding detection logic based on typical Arabic characters
            // This is a simplistic approach and might not be 100% accurate
            // You may need to refine or enhance this logic based on your specific requirements
            // For demonstration purposes, this method checks if the buffer contains any Arabic characters
            for (int i = 0; i < length - 1; i++)
            {
                // Check if the byte sequence resembles Arabic characters in Windows-1256 encoding range
                if ((buffer[i] >= 0xC1 && buffer[i] <= 0xDA) || (buffer[i] >= 0xE0 && buffer[i] <= 0xFA))
                {
                    return true;
                }
            }

            return false;
        }
    }