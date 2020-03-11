namespace CallCenter.Employee
{
    class Manager : Employeer
    {
        public Manager(int i)
        {
            Name = "Менеджер " + i.ToString();
            Level = 2;
        }
    }
}
