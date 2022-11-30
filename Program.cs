using System;
using SRO_TextDataString.Client;
using System.Data.SqlClient;
using System.IO;

namespace SRO_TextDataString
{
    internal class Program
    {
        private static Reader _mediapk2Reader;
        private static string _mediapk2Path = string.Empty;
        private const string ConnectionString = "Server=192.168.1.78;Initial Catalog=SILKROAD_R_CLIENT;User Id=sror_server;Password=169841;";

        private static void Main(string[] args)
        {
            Console.Title = "Client_TextData To Database";
            const string fileList = "textdata_equip&skill|textdata_object" +
                                    "|textevent|texthelp|textquest_otherstring" +
                                    "|textquest_queststring|textquest_speech&name" +
                                    "|textuisystem|textzonename";
            try
            {
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("Usage: Drag Media.pk2 here but make sure the dir name contains its language code. E.g. C:\\Silkroad_KR\\Media.pk2");
                    Console.WriteLine("Press any key or ESC to exit...");
                    Console.ReadKey();
                    return;
                }

                if (Path.GetFileName(args[0]).ToLower() != "media.pk2")
                {
                    Console.WriteLine("Drag a Media.pk2 here");
                    Console.ReadKey();
                    return;
                }

                _mediapk2Path = args[0];

                _mediapk2Reader = new Reader(_mediapk2Path, "169841");

                try
                {
                    foreach (var fileIn in fileList.Split('|'))
                    {
                        Console.WriteLine("{0}", $"{fileIn.ToLower()}.txt");
                        foreach (var file in _mediapk2Reader.GetFileText($"{fileIn}.txt").Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries))
                        {
                            ReadLineToDatabase(_mediapk2Path.Contains("_VN") ? file : _mediapk2Reader.GetFileText(file), fileIn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Completed!");
                Console.WriteLine("Press any key or ESC to exit...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        private static void ReadLineToDatabase(string data, string filein)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    
                    foreach (var row in data.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var errorAt = string.Empty;
                        try
                        {
                            if (row.StartsWith("//") || !row.StartsWith("1")) continue;

                            var rawCol = row.Split('\t');

                            var svc = rawCol[00];
                        
                            string str, kor, yyy, zzz, chn, twn, jpn;
                            string eng, vnm, tha, rus, tur, esp, egy, deu;

                            if (_mediapk2Path.Contains("_VN_3JOB"))
                            {
                                str = rawCol[01];
                                errorAt = str;
                                kor = rawCol[02].Replace("\'","\'\'");
                                yyy = rawCol[03].Replace("\'","\'\'");
                                zzz = rawCol[04].Replace("\'","\'\'");
                                chn = rawCol[05].Replace("\'","\'\'");
                                twn = rawCol[06].Replace("\'","\'\'");
                                jpn = rawCol[07].Replace("\'","\'\'");
                                eng = rawCol[08].Replace("\'","\'\'");
                                vnm = rawCol[09].Replace("\'","\'\'");
                                tha = rawCol[10].Replace("\'","\'\'");
                                rus = rawCol[11].Replace("\'","\'\'");
                                tur = rawCol[12].Replace("\'","\'\'");
                                esp = rawCol[13].Replace("\'","\'\'");
                                egy = rawCol[14].Replace("\'","\'\'");
                                deu = rawCol[15].Replace("\'","\'\'");
                            }
                            else
                            {
                                str = rawCol[02];
                                errorAt = str;
                                kor = rawCol[03].Replace("\'","\'\'");
                                yyy = rawCol[04].Replace("\'","\'\'");
                                zzz = rawCol[05].Replace("\'","\'\'");
                                chn = rawCol[06].Replace("\'","\'\'");
                                twn = rawCol[07].Replace("\'","\'\'");
                                jpn = rawCol[08].Replace("\'","\'\'");
                                eng = rawCol[09].Replace("\'","\'\'");
                                vnm = rawCol[10].Replace("\'","\'\'");
                                tha = rawCol[11].Replace("\'","\'\'");
                                rus = rawCol[12].Replace("\'","\'\'");
                                tur = rawCol[13].Replace("\'","\'\'");
                                esp = rawCol[14].Replace("\'","\'\'");
                                egy = rawCol[15].Replace("\'","\'\'");
                                deu = rawCol[16].Replace("\'","\'\'");
                            }

                            var cmd = new SqlCommand($"SELECT COUNT(*) FROM [_{filein}] WHERE [NameStrID128] = '{str}'", conn);

                            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                            {
                                //IMPORTANT!! Do the rigid media.pk2 after Silkroad Global because rigid is using EN also.
                                if (_mediapk2Path.Contains("_RIGID"))
                                {
                                    var check = new SqlCommand($"SELECT [EN] FROM [_{filein}] WHERE [NameStrID128] = '{str}'", conn);

                                    if (check.ExecuteScalar().ToString() == "0")
                                    {
                                        cmd = new SqlCommand($"UPDATE [_{filein}] SET [EN] = N'{eng}' WHERE [NameStrID128] = '{str}'", conn);
                                    }
                                }
                                if (_mediapk2Path.Contains("_KR"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [KR] = N'{kor}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_CN"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [CN] = N'{chn}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_TW"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [TW] = N'{twn}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_JP"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [JP] = N'{jpn}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_VN") || _mediapk2Path.Contains("_VN_3JOB"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [VN] = N'{vnm}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_TH"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [TH] = N'{tha}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_RU"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [RU] = N'{rus}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_TR"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [TR] = N'{tur}' WHERE [NameStrID128] = '{str}'", conn);
                                }
                                if (_mediapk2Path.Contains("_GB"))
                                {
                                    cmd = new SqlCommand($"UPDATE [_{filein}] SET [EN] = N'{eng}',[TR] = N'{tur}',[ES] = N'{esp}',[EG] = N'{egy}',[DE] = N'{deu}' WHERE [NameStrID128] = '{str}'", conn);
                                }

                                cmd.ExecuteNonQuery();
                                continue;
                            }
                       
                            cmd = new SqlCommand($"INSERT INTO [_{filein}] ([Service],[NameStrID128],[KR],[UNK1],[UNK2],[CN],[TW],[JP],[EN],[VN],[TH],[RU],[TR],[ES],[EG],[DE])" +
                                                 $"VALUES({svc},'{str}',N'{kor}',N'{yyy}',N'{zzz}',N'{chn}',N'{twn}',N'{jpn}',N'{eng}',N'{vnm}',N'{tha}',N'{rus}',N'{tur}',N'{esp}',N'{egy}',N'{deu}')", conn);

                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            Console.WriteLine($"Ignoring bullshit mistakes, double check this {errorAt}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}
