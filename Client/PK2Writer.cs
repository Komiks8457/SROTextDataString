using System.Runtime.InteropServices;

namespace SRO_TextDataString.Client
{
    public class PK2Writer
    {
        [DllImport("PK2Writer.dll", EntryPoint = "_Initialize@4", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Sets up GfxFileManager.DLL for PK2 operations. This function must be
        // called first.
        // bool Initialize(const char * gfxDllFilename);
        public static extern bool Initialize(string gfxDllFilename);

        [DllImport("PK2Writer.dll", EntryPoint = "_Deinitialize@0", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Cleans up GfxFileManager.DLL. This function must be called before the
        // program exits and after Close if a PK2 file was opened.
        // bool Deinitialize();
        public static extern bool Deinitialize();

        [DllImport("PK2Writer.dll", EntryPoint = "_Open@12", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Opens a PK2 file for writing. Use:
        //		"169841" - For official sro, mysro
        //		"\x32\x30\x30\x39\xC4\xEA" - for zszc, swsro
        // Refer to this guide:
        // http:'www.elitepvpers.de/forum/sro-guides-templates/612789-guide-finding-pk2-blowfish-key-5-easy-steps.html
        // To get the "base key"
        // bool Open(const char * pk2Filename, void * accessKey, unsigned char accessKeyLen);
        public static extern bool Open(string pk2Filename, string accessKey, int accessKeyLen);

        [DllImport("PK2Writer.dll", EntryPoint = "_Close@0", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Closes an opened PK2 file. This function must be called before the program
        // exits and before Deinitialize is called.
        // bool Close();
        public static extern bool Close();

        [DllImport("PK2Writer.dll", EntryPoint = "_ImportFile@8", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Imports a file to the PK2. 'entryFilename' should be the full path the
        // file should have in the PK2.
        // bool ImportFile(const char * entryFilename, const char * inputFilename);
        public static extern bool ImportFile(string entryFilename, string inputFilename);
    }
}