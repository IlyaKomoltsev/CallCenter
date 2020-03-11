namespace CallCenter.Employee
{
    internal class Operator : Employeer
    {
        public Operator(int i) 
        {
            Name = "Оператор " + i.ToString();
            Level = 1;
        }
    }
}
