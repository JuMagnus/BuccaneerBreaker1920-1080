using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using static BuccaneerBreaker.Services.LevelsManager;

namespace BuccaneerBreaker.Services
{

    public interface Ilevels
    {
        public int currentLevel { get; set; }
        public void LoadAllLevels(string repo);
        public SingleLevel GetLevel(int level);


    }
    
    public class LevelsManager : Ilevels
    {

        private List<SingleLevel> _levels;
        
        //variable que va modifier le choix du niveau pour que le scène manager sache quel niveau charger
        public int currentLevel { get; set; }
        public LevelsManager()
        {
            _levels = new List<SingleLevel>();
            ServicesLocator.Register<Ilevels>(this);
        }

        //Récupère toutes les infos des fichiers json stockés dans le dossier Levels afin de remplir le tableau de données _levels depuis le Main 
        public void LoadAllLevels(string repo)
        {
            string[] files = Directory.GetFiles(repo);
            MemoryStream ms;            
            for (int i = 0; i < files.Length; i++)
            {
                ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(files[i])));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SingleLevel));
                _levels.Add((SingleLevel)serializer.ReadObject(ms));
            }
        }

        //Récupère le niveau en cours 
        public SingleLevel GetLevel(int level)
        {
            return _levels[level];
        }


        [DataContract]
        public class SingleLevel
        {
            [DataMember]
            public int levelNumber { get; set; }
            
            [DataMember]
            public string texture { get; set; }

            [DataMember]
            public string music { get; set; }
            
            [DataMember]
            public List<List<int>> bricks { get; private set; }
        }

    }
}
