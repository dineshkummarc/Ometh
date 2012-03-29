namespace Ometh.Core
{
    public class Person
    {
        public string Name { get; private set; }

        public string Email { get; private set; }

        public Person(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }
    }
}