sharp_records
=============

A very simple active records library for c#.
You can download the dll here:
https://www.dropbox.com/sh/pzwuwubsx14ymhh/AADQ9-b-YXXB6r18beLDo_S1a?dl=0

Comming Features
===
The following features will be added soon:
- Better search
- Custom Tables
- Custom Settings

How to Use it
===
First create the sql table (Here named exampleitem, always lowercase)

```SQL
CREATE TABLE [dbo].[exampleitem] ( 
   [id]    INT           IDENTITY (1, 1) NOT NULL, 
   [value] VARCHAR (255) DEFAULT ('') NULL, 
   PRIMARY KEY CLUSTERED ([id] ASC) 
); 
```
Then create a model (Here named ExampleItem after the table)
```C#
public class ExampleItem : DK.Active_Records.DBModel<ExampleItem> , DK.Active_Records.IDBModel 
{  
  public ExampleItem() : base() { } 
} 
```
Then go to the main function and first open the connection
```C#
DK.Active_Records.SQL.connect(Environment.CurrentDirectory + "\\data.mdf");
```
(and close at the end)
```C#
DK.Active_Records.SQL.close();
```
To read all items you have to do this:
```C#
List<ExampleItem> items = ExampleItem.getAll();
```
If you wanna do searchs/wheres you can do this:
```C#
// Where Raw
List<ExampleItem> items = ExampleItem.where(" value = 'test'");
// Where
List<ExampleItem> items = ExampleItem.where("value","test",whereTyp.equal);
// Search
List<ExampleItem> items = ExampleItem.search("test","column1","columnd2","column3",...);
```
To add a new item do this:
```C#
ExampleItem item = new ExampleItem(); 
item["value"] = "added"; 
item.save(); 
```
To update an item do this (items is the list of before):
```C#
items[0]["value"] = "changed"; 
items[0].save(); 
```
To delete an item do this (items is the list of before):
```C#
items[0].delete(); 
```
