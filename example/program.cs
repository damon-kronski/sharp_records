using DK.Active_Records;

namespace DK.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Open SQL Connection
            SQL.connect(Environment.CurrentDirectory + "\\data.mdf");

            // Get All Items
            List<Models.ExampleItem> items = Models.ExampleItem.getAll();

            // Write All Values
            foreach (var i in items)
                Console.WriteLine("After: " + i["value"].ToString());

            // Change first item value to changed and save
            items[0]["value"] = "changed";
            items[0].save();

            // Create new item, set value and save
            Models.ExampleItem item = new Models.ExampleItem();
            item["value"] = "added";
            item.save();

            // Get All Items
            items = Models.ExampleItem.getAll();

            // Write All Values
            foreach (var i in items)
                Console.WriteLine("After: " + i["value"].ToString());

            // Close SQL Connection
            SQL.close();
            
            // Wait for User input before close
            Console.ReadLine();
        }
    }
}
