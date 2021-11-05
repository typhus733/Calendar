using System;
using System.IO;
using System.Collections.Generic;

namespace Calendar
{
     public class Meeting
    {
        private string title;
        private string location;
        private DateTime startDateTime;
        private DateTime endDateTime;
        private Contact contact1;

        public Meeting(string title, string location, DateTime start, DateTime end)
        {
            Title = title;
            Location = location;
            StartDateTime = start;
            EndDateTime = end;
        }

        public Meeting(string title, string location, DateTime start, DateTime end, Contact contact)
        {
            Title = title;
            Location = location;
            StartDateTime = start;
            EndDateTime = end;
            Contact1 = contact;
        }

        public Contact Contact1
        {
            get
            {
                return this.contact1;
            }
            set
            {
                this.contact1 = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return this.startDateTime;
            }
            set
            {
                this.startDateTime = value;
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return this.endDateTime;
            }
            set
            {
                this.endDateTime = value;
            }
        }
    }


    public class Contact
    {
        private string firstName;
        private string lastName;
        private string phone;
        private string email;

        public Contact(string firstName, string lastName, string phone, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = value;
            }
        }

        public string Phone
        {
            get
            {
                return this.phone;
            }
            set
            {
                this.phone = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        public override string ToString()
        {
            return (FirstName + " " + LastName + " - " + Phone + " - " + Email);
        }
    }


    class Calendar
    {
        static List<Meeting> CalendarFill(string[] calendar) 
        {
            List<Meeting> currentCalendar = new List<Meeting>();

            foreach (string line in calendar)
            {
                
                string[] lineSplit = line.Split(",");
                Meeting meetingLine = new Meeting(lineSplit[0], lineSplit[1], DateTime.Parse(lineSplit[2]), DateTime.Parse(lineSplit[3]));
                currentCalendar.Add(meetingLine);
            }

            return currentCalendar;
        }

        static List<Contact> ContactFill()
        {
            List<Contact> contactList = new List<Contact>();
            string[] contactStrings = File.ReadAllLines(@"C:\Users\brandon.gunthner\Desktop\contacts.dat");


            foreach (string line in contactStrings)
            {

                string[] lineSplit = line.Split(",");
                Contact contactLine = new Contact(lineSplit[0], lineSplit[1], lineSplit[2], lineSplit[3]);
                contactList.Add(contactLine);
            }

            return contactList;
        }


        static void GetEvent(List<Meeting> calendar)
        {
            Console.WriteLine("Enter the event Title:");
            string title = Console.ReadLine();
            Console.WriteLine("Enter the event Location:");
            string location = Console.ReadLine();
            Console.WriteLine("Enter the event Start date (formatted MM/DD/YYYY Hour:Minute AM/PM):");
            DateTime start = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter the event End date (formatted MM/DD/YYYY Hour:Minute AM/PM):");
            DateTime end = DateTime.Parse(Console.ReadLine());
            Meeting newEvent = new Meeting(title, location, start, end);

            foreach(Meeting meeting in calendar)
            {
                if ((newEvent.StartDateTime >= meeting.StartDateTime && newEvent.StartDateTime <= meeting.EndDateTime) ||
                    (newEvent.StartDateTime <= meeting.StartDateTime && newEvent.EndDateTime > meeting.StartDateTime))
                {
                    Console.WriteLine("\nWarning: New event overlaps with " + meeting.Title + " (" + meeting.StartDateTime.ToString() + " - " + meeting.EndDateTime.ToString() + ")\n");
                    Console.WriteLine("Continue to add? 1 to continue or any other key to cancel:");
                    string confirm = Console.ReadLine();

                    if(confirm == "1")
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("\nEvent add canceled\n");
                        return;
                    }
                }
            }

            calendar.Add(newEvent);
            Console.WriteLine("\nEvent add completed successfully\n");
        }

        static void CalendarPrint(List<Meeting> calendar)
        {
            DateTime today = DateTime.Today;
            for (today = today.AddHours(8); today.Hour <= 17; today = today.AddMinutes(30))
            {
                Console.Write(today.ToString());
                foreach(Meeting meeting in calendar)
                {
                    if (meeting.StartDateTime <= today && meeting.EndDateTime > today)
                    {
                        Console.Write(" | " + meeting.Title + " at " + meeting.Location);
                    }
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        static void CalendarSave(List<Meeting> calendar, string location)
        {
            string saveString = "";
            foreach (Meeting meeting in calendar)
            {
                saveString += (meeting.Title + "," + meeting.Location + "," + meeting.StartDateTime.ToString() + "," + meeting.EndDateTime.ToString() + "\n");
            }
            File.WriteAllText(location, saveString);
            Console.WriteLine("\nSaved Successfully\n");
        }

        static void ContactsSave(List<Contact> contacts)
        {
            string saveString = "";
            foreach (Contact contact in contacts)
            {
                saveString += (contact.FirstName + "," + contact.LastName + "," + contact.Phone + "," + contact.Email + "\n");
            }
            File.WriteAllText(@"C:\Users\brandon.gunthner\Desktop\contacts.dat", saveString);
            Console.WriteLine("\nSaved Successfully\n");
        }

        static void ContactPrint(List<Contact> contacts)
        {
            foreach (Contact contact in contacts)
            {
                Console.WriteLine("{0} {1} - {2} - {3}", contact.FirstName, contact.LastName, contact.Phone, contact.Email);
            }
            Console.WriteLine("-----------------\n");
        }

        static void ContactAdd(List<Contact> contacts)
        {
            Console.Write("Enter Contact First Name: ");
            string first = Console.ReadLine();
            Console.Write("Enter Contact Last Name: ");
            string last = Console.ReadLine();
            Console.Write("Enter Contact Phone Number: ");
            string phone = Console.ReadLine();
            Console.Write("Enter Contact Email: ");
            string email = Console.ReadLine();
            foreach (Contact contact in contacts)
            {
                if (contact.FirstName.ToLower() == first.ToLower() && contact.LastName.ToLower() == last.ToLower() 
                    && contact.Phone.ToLower() == phone.ToLower() && contact.Email.ToLower() == email.ToLower())
                {
                    Console.WriteLine("Contact already exists, function canceled\n");
                    return;
                }
            }

            Contact newContact = new Contact(first, last, phone, email);

            contacts.Add(newContact);

            Console.WriteLine("\nContact added successfully\n");
        }

        static void ContactRemove(List<Contact> contacts)
        {
            Console.Write("Enter Contact First Name: ");
            string first = Console.ReadLine();
            Console.Write("Enter Contact Last Name: ");
            string last = Console.ReadLine();
            for (int x = contacts.Count - 1; x > -1; x--)
            {
                if (contacts[x].FirstName.ToLower() == first.ToLower() && contacts[x].LastName.ToLower() == last.ToLower())
                {
                    Console.WriteLine("Delete Contact: {0} {1} - {2} - {3}", contacts[x].FirstName, contacts[x].LastName, contacts[x].Phone, contacts[x].Email);
                    Console.Write("Enter Y to confirm, any other key to not delete contact: ");
                    string confirm = Console.ReadLine();

                    if (confirm == "Y" || confirm == "y")
                    {
                        contacts.RemoveAt(x);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

        }

        static void CalendarMenu(List<Meeting> calendar)
        {
            bool run = true;
            do
            {
                Console.WriteLine("Enter a menu option 1-4, calendar will be saved on exit:\n1 - Add event\n2 - Remove event\n3 - Display calendar\n4 - Exit menu");
                string menuInput = Console.ReadLine();
                switch (menuInput)
                {
                    case "1":
                        try
                        {   
                            GetEvent(calendar);
                            
                        }
                        catch(FormatException e)
                        {
                            Console.WriteLine("\nInvalid date in event, failed to add\n");
                        }
                        break;
                    case "2":
                        Console.WriteLine("\nEnter the event Title to remove:");
                        string title = Console.ReadLine();
                        int found = 0;
                        for (int x = calendar.Count-1; x > -1; x--)
                        {   
                            if (calendar[x].Title.ToLower() == title.ToLower())
                            {
                                calendar.RemoveAt(x);
                                found = 1;
                                Console.WriteLine("\nEvent successfully removed\n");
                            }
                        }
                        if (found == 0)
                        {
                            Console.WriteLine("\nEvent not found in calendar, failed to remove\n");
                        }
                        break;
                    case "3":
                        CalendarPrint(calendar);
                        break;
                    case "4":
                        run = false;
                        break;
                    default:
                        Console.WriteLine("\nInput not recognized\n");
                        break;

                }

            } while (run == true);
        }

        static void ContactMenu(List<Contact> contacts)
        {
            bool run = true;
            do
            {
                try
                {
                    Console.WriteLine("Enter a menu option 1-4:\n1 - View Contacts\n2 - Make a new Contact\n3 - Remove a Contact\n4 - Exit menu and save");
                    string menuInput = Console.ReadLine();
                    switch (menuInput)
                    {
                        case "1":
                            Console.WriteLine("-----------------");
                            ContactPrint(contacts);
                            break;
                        case "2":
                            ContactAdd(contacts);
                            break;
                        case "3":
                            ContactRemove(contacts);
                            break;
                        case "4":
                            ContactsSave(contacts);
                            run = false;
                            break;
                        default:
                            Console.WriteLine("Input not valid");
                            break;

                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error updating contacts, check file");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error updating contacts, check file");
                }
            }
            while (run == true);
        }
        
        static void Main(string[] args)
        {
            bool run = true;
            List<Meeting> currentCalendar = new List<Meeting>();
            List<Contact> contactList = new List<Contact>();
            try
            {
                contactList = ContactFill();
            }
            catch
            {
                Console.WriteLine("Warning! Error reading contacts, check file");
            }

            do
            {
                Console.WriteLine("Enter a menu option 1-4:\n1 - Load an existing Calendar\n2 - Make a new Calendar\n3 - Add/View Contacts\n4 - Exit program");
                string topLevelInput = Console.ReadLine();
                try
                {
                    switch (topLevelInput)
                    {
                        case "1":

                            Console.WriteLine("Enter the location for the calendar file:");
                            string fileLocation = Console.ReadLine();
                            string[] calendarStrings = File.ReadAllLines(@fileLocation);
                            currentCalendar = CalendarFill(calendarStrings);
                            Console.WriteLine("\nFile read successfully\n");
                            CalendarMenu(currentCalendar);
                            CalendarSave(currentCalendar, fileLocation);
                            break;

                        case "2":

                            Console.WriteLine("Enter a location to save calendar:");
                            string saveLocation = Console.ReadLine();
                            CalendarMenu(currentCalendar);
                            CalendarSave(currentCalendar, saveLocation);
                            break;

                        case "3":
                            Console.WriteLine("");
                            ContactMenu(contactList);
                            break;

                        case "4":

                            run = false;
                            break;

                        default:

                            Console.WriteLine("Input not recognized");
                            break;
                    }
                }
                catch(IOException e) 
                {
                    Console.WriteLine("Error handling file, could not open/save\n");  
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Invalid data found in file, could not populate calendar\n");
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error populating calendar\n");
                }
            }
            while (run == true);
        }
    }
}
