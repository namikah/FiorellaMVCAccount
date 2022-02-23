using MySettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Settings
{
    public static class Settings
    {
        static List<string> sliderImageSettings = new() { SliderImage.MaxImageCount().ToString() };


        File.WriteAllText(@"C:\Users\EshginK\Desktop\students.json", JsonConvert.SerializeObject(sliderImageSettings));

                public static int MaxImageCount()
        {
            var readedJson = File.ReadAllText(@"C:\Users\EshginK\Desktop\students.json");
            var deserializedStudents = JsonConvert.DeserializeObject<List<Student>>(readedJson);

            return
                }

        var readedJson = File.ReadAllText(@"C:\Users\EshginK\Desktop\students.json");
        var deserializedStudents = JsonConvert.DeserializeObject<List<Student>>(readedJson);

                    foreach (var student in deserializedStudents.Skip(1))
                    {
                        Console.WriteLine(student.Id + " - " + student.Name + " - " + student.Surname);
                    }

}
}