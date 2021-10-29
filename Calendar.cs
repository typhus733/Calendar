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

        public Meeting(string title, string location, DateTime start, DateTime end)
        {
            Title = title;
            Location = location;
            StartDateTime = start;
            EndDateTime = end;
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
    
    
    class Calendar
    {
        static string[] ReadCalendar(string location)
        {
            bool input = true;
            do
            {
                try
                {
                    string[] readFile = File.ReadAllLines(@location);
                    return readFile;
                }
                catch (IOException e)
                {
                    Console.WriteLine("File not found/Bad file. Enter 1 to try again or any other key to exit:");
                    string exitConfirm = Console.ReadLine();
                    if (exitConfirm == "1")
                    { continue; }
                    else
                    {
                        input = false;
                    }
                }
            }
            while (input == true);
            string[] empty = new string[0];
            empty[0] = "nul";
            return empty;
        }
        
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

        static Meeting GetEvent(List<Meeting> calendar)
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
                    Console.WriteLine("Warning: New event overlaps with " + meeting.Title + " (" + meeting.StartDateTime.ToString() + " - " + meeting.EndDateTime.ToString() + ")");
                }
            }

            return newEvent;
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
                        Console.Write(" | " + meeting.Title + " at " + meeting.Location + "\n");
                    }
                }
                Console.Write("\n");
            }

        }

        static void CalendarMenu(List<Meeting> calendar)
        {
            bool run = true;
            do
            {
                Console.WriteLine("Enter a menu option 1-4, calendar will be saved on exit:\n1 - Add event, 2 - Remove event, 3 - Display calendar, 4 - Exit menu");
                string menuInput = Console.ReadLine();
                switch (menuInput)
                {
                    case "1":
                        try
                        {   
                            calendar.Add(GetEvent(calendar));
                        }
                        catch(FormatException e)
                        {
                            Console.WriteLine("Invalid date in event, failed to add");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Enter the event Title to remove:");
                        string title = Console.ReadLine();
                        int found = 0;
                        foreach(Meeting meeting in calendar)
                        {
                            if (meeting.Title == title)
                            {
                                calendar.Remove(meeting);
                                found = 1;
                                Console.WriteLine("Event successfully removed");
                            }
                        }
                        if (found == 0)
                        {
                            Console.WriteLine("Event not found in calendar, failed to remove");
                        }
                        break;
                    case "3":
                        CalendarPrint(calendar);
                        break;
                    case "4":
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Input not recognized");
                        break;

                }

            } while (run == true);
        }

        static void CalendarSave(List<Meeting> calendar, string location)
        {
            string saveString = "";
            foreach (Meeting meeting in calendar)
            {
                saveString += (meeting.Title + " , " + meeting.Location + " , " + meeting.StartDateTime.ToString() + " , " + meeting.EndDateTime.ToString() + "\n");
            }
            File.WriteAllText(location, saveString);
            Console.WriteLine("Saved Successfully");
        }
        
        static void Main(string[] args)
        {
            bool run = true;
            List<Meeting> currentCalendar = new List<Meeting>();
            do
            {
                Console.WriteLine("Enter a menu option 1-3:\n1 - Load an existing Calendar, 2 - Make a new Calendar, 3 - Exit program");
                string topLevelInput = Console.ReadLine();

                switch (topLevelInput)
                {
                    case "1":
                        try
                        {
                            Console.WriteLine("Enter the file location for the calendar:");
                            string fileLocation = Console.ReadLine();
                            string[] calendarStrings = ReadCalendar(@fileLocation);
                            if (calendarStrings[0] == "nul")
                            {
                                break;
                            }
                            else
                            {
                                currentCalendar = CalendarFill(calendarStrings);
                                Console.WriteLine("File read successfully");
                            }
                            CalendarMenu(currentCalendar);
                            CalendarSave(currentCalendar, fileLocation);
                            break;
                        }
                        catch(FormatException e)
                        {
                            Console.WriteLine("Invalid data found in file, could not populate calendar");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error populating calendar\n");
                            break;
                        }
            
                    case "2":
                        try
                        {
                            Console.WriteLine("Enter a location to save calendar:");
                            string saveLocation = Console.ReadLine();
                            CalendarMenu(currentCalendar);
                            CalendarSave(currentCalendar, saveLocation);
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error populating calendar\n");
                            break;
                        }
                    case "3":
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Input not recognized");
                        break;
                }
            }
            while (run == true);
        }
    }
}
