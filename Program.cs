using task2;




DMFactory factory = new DMFactory();

DeviceManager dm = factory.CreateDM("C:\\Users\\exige\\RiderProjects\\NET_project2\\NET_project2\\input.txt");

dm.editDevice(new PersonalComputer("0","new",true, "sigma"),"2");
dm.turnOn("2");
Console.WriteLine(dm.ToString());
dm.saveStorage("C:\\Users\\exige\\RiderProjects\\NET_project2\\NET_project2\\output.txt");
