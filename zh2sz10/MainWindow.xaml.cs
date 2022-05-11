using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zh2sz10
{
    
    public partial class MainWindow : Window
    {   
        
        MongoCRUD db = new MongoCRUD("NetflixShows");
        List<string> options = new List<string>();
        List<string> options2 = new List<string>();
        string tableName = "Shows";
        string filePath = "shows.json";
        public MainWindow()
        {
            InitializeComponent();
            mainWindow.Title = "Netflix Shows";
            mainWindow.MinHeight = SystemParameters.FullPrimaryScreenHeight;
            mainWindow.MaxHeight = SystemParameters.FullPrimaryScreenHeight;
            mainWindow.MinWidth = SystemParameters.FullPrimaryScreenWidth - 100;
            mainWindow.MaxWidth = SystemParameters.FullPrimaryScreenWidth - 100;
            var records = db.LoadRecords<Show>(tableName);
            dataGrid.ItemsSource = records;
            Options();
            FilterOptions();
            fileName.IsReadOnly = true;
        }
        private void Options()
        {
            options.Add("show_id");
            options.Add("type");
            options.Add("title");
            options.Add("director");
            options.Add("cast");
            options.Add("country");
            options.Add("release_year");

            nameFilterOptions.ItemsSource = options;
            nameFilterOptions.SelectedIndex = 0;
        }
        private void FilterOptions()
        {
            options2.Add("Equals");
            options2.Add("Contains");
           
            filterOptions.ItemsSource = options2;
            filterOptions.SelectedIndex = 0;
        }
        private void jsonReader(string path)
        {
            dynamic jsonFile = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(path));
            int i = 0;
            JToken n1 = jsonFile.SelectToken("[" + i + "].show_id");
            JToken n2 = jsonFile.SelectToken("[" + i + "].type");
            JToken n3 = jsonFile.SelectToken("[" + i + "].title");
            JToken n4 = jsonFile.SelectToken("[" + i + "].director");
            JToken n5 = jsonFile.SelectToken("[" + i + "].cast");
            JToken n6 = jsonFile.SelectToken("[" + i + "].country");
            JToken n7 = jsonFile.SelectToken("[" + i + "].date_added");
            JToken n8 = jsonFile.SelectToken("[" + i + "].release_year");
            JToken n9 = jsonFile.SelectToken("[" + i + "].rating");
            JToken n10 = jsonFile.SelectToken("[" + i + "].duration");
            JToken n11 = jsonFile.SelectToken("[" + i + "].listed_in");
            JToken n12 = jsonFile.SelectToken("[" + i + "].description");
            

            //int show_id, string type, string title, string director, string cast, 
            //string country, string date_added,
            //int release_year, string rating, string duration, string listed_in, string description
            Show s = new Show(int.Parse(n1.ToString()),n2.ToString(),n3.ToString(),n4.ToString(),n5.ToString(),n6.ToString()
                ,n7.ToString(),int.Parse(n8.ToString()),n9.ToString(),n10.ToString(),n11.ToString(),n12.ToString());
            

            while (jsonFile.SelectToken("[" + i + "].show_id") != null)
            {
                 n1 = jsonFile.SelectToken("[" + i + "].show_id");
                 n2 = jsonFile.SelectToken("[" + i + "].type");
                 n3 = jsonFile.SelectToken("[" + i + "].title");
                 n4 = jsonFile.SelectToken("[" + i + "].director");
                 n5 = jsonFile.SelectToken("[" + i + "].cast");
                 n6 = jsonFile.SelectToken("[" + i + "].country");
                 n7 = jsonFile.SelectToken("[" + i + "].date_added");
                 n8 = jsonFile.SelectToken("[" + i + "].release_year");
                 n9 = jsonFile.SelectToken("[" + i + "].rating");
                 n10 = jsonFile.SelectToken("[" + i + "].duration");
                 n11 = jsonFile.SelectToken("[" + i + "].listed_in");
                 n12 = jsonFile.SelectToken("[" + i + "].description");
                
                //int show_id, string type, string title, string director, string cast, 
                //string country, string date_added,
                //int release_year, string rating, string duration, string listed_in, string description
                s = new Show(int.Parse(n1.ToString()), n2.ToString(), n3.ToString(), n4.ToString(), n5.ToString(), n6.ToString()
                    , n7.ToString(), int.Parse(n8.ToString()), n9.ToString(), n10.ToString(), n11.ToString(), n12.ToString());
                db.InsertRecord(tableName,s);



                i++;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                jsonReader(filePath);
                var records = db.LoadRecords<Show>(tableName);
                dataGrid.ItemsSource = records;
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            db.EmptyTable<Show>(tableName);
            var records = db.LoadRecords<Show>(tableName);
            dataGrid.ItemsSource = records;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Filter
            string opt = nameFilterOptions.SelectedItem.ToString();
            string s = textBox.Text;
            string opt2 = filterOptions.SelectedItem.ToString();
            var items = db.LoadRecordsByFilter<Show>(tableName, opt, s, opt2);
            if (String.IsNullOrEmpty(s))
            {
                dataGrid.ItemsSource = db.LoadRecords<Show>(tableName);


            }
            else
            {
                dataGrid.ItemsSource = items;

            }
        }

        private void nameFilterOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                    
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                
                    fileName.Text = openFileDlg.FileName;
                    filePath = fileName.Text;
                
            }

        }

        private void fileName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    public class MongoCRUD
    {
        private IMongoDatabase db;
        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
        public void EmptyTable<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            this.db.DropCollection(table);
            
        }

        public List<T> LoadRecordsByFilter<T>(string table, string f, string s, string option)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq(f, s);

            if (option == "Equals")
            {
                filter = Builders<T>.Filter.Eq(f, s);
            }

            else if (option == "Contains")
            {
                filter = Builders<T>.Filter.Regex(f, new BsonRegularExpression(s));
            }
            return collection.Find(filter).ToList();
        }
        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);

            var result = collection.ReplaceOne(new BsonDocument("_id", id), record, new UpdateOptions { IsUpsert = true });

        }
        public void DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
