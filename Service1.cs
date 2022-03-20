using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;

namespace SoftForConemu{
    class iterations{
        public static string[] iter(string path){
            string line;
            string[] old_data = new string[1];

            using(StreamReader sr = new StreamReader(path, Encoding.UTF8)){
                while((line = sr.ReadLine()) != null){
                    if(line.IndexOf("name=") > -1 && line.IndexOf("BackGround") > -1 && line.IndexOf("Image") > -1 && line.IndexOf("string") > -1){
                        old_data[0] = line;
                        break;
                    }
                }
            }

            string folder_string = string.Empty;
            string middle_folder = string.Empty;

            for(int i = 57; i < old_data[0].Length - 3; ++i){
                folder_string += old_data[0][i];
            }

            for(int i = 0; i < 63; ++i){
                middle_folder += old_data[0][i];
            }

            string end_folder = "";

            end_folder += Convert.ToString('"') + @"/>";

            string folder_string_copy = string.Empty;

            for (int i = 6; i < folder_string.Length; ++i){
                folder_string_copy += folder_string[i];
            }

            Variables.path_list.Remove(folder_string_copy);

            string random_string = Variables.path_list[Variables.rn.Next(0, Variables.path_list.Count())];
            string new_folder_name = middle_folder + random_string + end_folder;
            string[] str = {new_folder_name, old_data[0]};

            return str;
        }

        public static void replacement(string path, string[] new_string){
            string line;
            string old_data = string.Empty;
            string new_data = string.Empty;

            using(StreamReader sr = new StreamReader(path, Encoding.UTF8)){
                while((line = sr.ReadLine()) != null){
                    old_data += line + "\n";
                }
            }

            new_data = old_data.Replace(new_string[1], new_string[0]);

            using(StreamWriter sw = new StreamWriter(path, false)){
                sw.WriteLine(new_data);
            }
        }
    }

    class Variables{
        //path to settings conemu
        static public string path = @"C:\Users\kenta\AppData\Roaming\ConEmu.xml";
        //path to the pack of pictures
        static public string image_path = @"C:\Users\kenta\Pictures\full";
        static public Random rn = new Random();
        static public string folder_data = string.Empty;
        static public string gtx = string.Empty;
        static public string random_image = string.Empty;
        static public string[] all_images_in_dir = Directory.GetFiles(image_path);
        static public List<string> path_list = new List<string>(){};

        static public void appending(){
            foreach(string i in all_images_in_dir){path_list.Add(Convert.ToString(i));}
        }
    }

    class first{
        public static void name(){
            Variables.appending();

            string[] new_string = iterations.iter(Variables.path);

            iterations.replacement(Variables.path, new_string);
        }
    }

    class logic{
        public static List<Process> process = null;
        public static void GetProc(){
            process = Process.GetProcesses().ToList<Process>();
        }

        public static bool check(List<int> array_id, int new_id){
            bool[] value = {true};

            if(array_id.Count() > 0){
                foreach(var i in array_id){
                    if (i == new_id){
                        value[0] = false;
                    }
                }
            }

            return value[0];
        }
    }

    public partial class Service1 : ServiceBase{
        public Service1(){
            InitializeComponent();
        }

        protected override void OnStart(string[] args){
            List<int> conemu_list = new List<int>();
            bool[] value_string = {false};

            while(true){
                logic.GetProc();

                foreach(var i in logic.process){
                    if(i.ProcessName == "ConEmu64"){
                        value_string[0] = true;

                        if(logic.check(conemu_list, i.Id)){
                            conemu_list.Add(i.Id);
                            first.name();
                            break;
                        }
                    }
                }

                if(!value_string[0]){
                    conemu_list.Clear();
                }

                Variables.path_list.Clear();
                value_string[0] = false;
                logic.process = null;
                Thread.Sleep(3333);
            }
        }
    }
}
