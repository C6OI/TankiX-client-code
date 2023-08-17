using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Platform.Library.ClientResources.API {
    public class FileUtils {
        public static void ReplaceLineInFile(string fileName, string contains, string target) {
            StreamReader streamReader = null;
            StreamWriter streamWriter = null;
            string text = null;
            string[] array = File.ReadAllLines(fileName);

            for (int i = 0; i < array.Count(); i++) {
                if (array[i].Contains(contains)) {
                    array[i] = target;
                }
            }

            File.WriteAllLines(fileName, array);
        }

        public static void DeleteDirectory(string directory) {
            if (Directory.Exists(directory)) {
                string[] directories = Directory.GetDirectories(directory);

                foreach (string directory2 in directories) {
                    DeleteDirectory(directory2);
                }

                string[] files = Directory.GetFiles(directory);

                foreach (string path in files) {
                    File.Delete(path);
                }

                Directory.Delete(directory);
            }
        }

        public static IEnumerable<string> GetFiles(string fromPath, Func<string, bool> filter = null) {
            string[] directories = Directory.GetDirectories(fromPath + "/");

            foreach (string directory in directories) {
                foreach (string file in GetFiles(directory, filter)) {
                    if (filter == null || filter(file)) {
                        yield return file;
                    }
                }
            }

            string[] files = Directory.GetFiles(fromPath + "/");

            foreach (string file2 in files) {
                if (filter == null || filter(file2)) {
                    yield return file2;
                }
            }
        }
    }
}