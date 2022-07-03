# ToDoList

#Comands
//CREATE
ToDoList.exe Create --Title Test {"Title"}  --Date {Date}. Result = Creates a new record.

//READ
ToDoList.exe Read - No Options specified gets all records.
ToDoList.exe Read --Filter {"SearchTerm"}. Result = Matching records by Title.
ToDoList.exe Read --OrderBy "Y". Result = Order results by Date Descending.


//UPDATE
ToDoList.exe Update --Title {"Title"}. Result = Updates all records matching title to IsComplete = true.
ToDoList.exe Update --ForDate 03/07/2022. Result = Updates all records matching date WHERE IsComplete is not already true.

//EXPORT
ToDoList.exe Export --Type {".txt"}. Result = Outputs the database into text file in the root of the directory. Saved as JSON for ease of use.
