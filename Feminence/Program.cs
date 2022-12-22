using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Net.Mail;
using System.Threading;

namespace Feminence
{
    internal class Program
    {
        const int minPasswordLength = 8;
        const string specialCharacters = "@#$%^&(/)";

        //Password Validation Method for New Account//
        static bool validatePassword(string password)
        {
            // Buckley, D. (2021).C# Password Validator 1. [online] www.youtube.com. Available at: https://www.youtube.com/watch?v=JIIeuhtTdpY [Accessed 17 Dec. 2022].
            bool hasLength = password.Length >= minPasswordLength;
            if (!hasLength) Console.WriteLine("Invalid, not long enough.");

            bool hasSpecialCharacter = false;
            foreach (char c in specialCharacters)
            {
                if (password.Contains(c))
                {
                    hasSpecialCharacter = true;
                    break;
                }
            }
            if (!hasSpecialCharacter) Console.WriteLine("Needs at least one special character" + specialCharacters + " to be valid");


            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumeral = false;
            for (int i = 0; i < password.Length; i++)
            {
                char s = password[i];
                if (Char.IsDigit(password[i]))
                {
                    hasNumeral = true;
                }
                else if (Char.IsLower(password[i]))
                {
                    hasLower = true;
                }
                else if (Char.IsUpper(password[i]))
                {
                    hasUpper = true;
                }
                else if (!hasUpper || !hasLower || !hasNumeral)
                {
                    Console.WriteLine("Needs a number, an uppercase and a lowercase to be valid.");
                }
            }
            return hasLength && hasSpecialCharacter && hasUpper && hasLower;
        }


        //Email Validation Method for New Account, checks if Email could be valid but not whether it actually exists
        //Woods, S. (2014). C# Tutorial - How to validate an e-mail address. [online] www.youtube.com. Available at: https://www.youtube.com/watch?v=os3NHojVvjU&t=22s [Accessed 18 Dec. 2022].
        public static bool isValidEmail(string checkEmail)
        {
            try
            {
                MailAddress mail = new MailAddress(checkEmail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static DateTime readDate(string message)
        {
            bool validDate = false;
            int year = 0;
            int month = 0;
            int day = 0;
            DateTime dateTime = new DateTime();
            while (!validDate)
            {
                Console.Write(message + ". Use this format dd/MM/yyyy, Eg. 01/09/1999: ");
                string input = Console.ReadLine().Replace(" ", "");
                string[] splitInput = input.Split(new char[] { '/' });

                if (splitInput.Length != 3)
                {
                    continue;
                }
                try
                {
                    year = int.Parse(splitInput[2]);
                    month = int.Parse(splitInput[1]);
                    day = int.Parse(splitInput[0]);
                    dateTime = new DateTime(year, month, day);
                    validDate = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid input for day, month or year. Please check correct format");
                }
                catch
                {

                }
            }
            return dateTime;
        }


        // New & Existing User Login Process//
        static void appStartup()
        {

            Console.WriteLine("Select a number from the following options: \n\t 1. SIGN UP (New Users) \t 2. SIGN IN (Existing Users)");
            Console.WriteLine();
            int welcomeUserResponse = int.Parse(Console.ReadLine());

            int signUp = 1;
            int signIn = 2;

            StreamWriter feminenceData = new StreamWriter(@"clients'data.csv"); // use to store users details in an excel file//
            while (true)
            {

                if (welcomeUserResponse == signUp)
                {
                    Console.WriteLine();
                    Console.Write("Enter your email address: ");
                    string userEmail = Console.ReadLine();

                    //validate e-mail address
                    while (!isValidEmail(userEmail))
                    {
                        Console.Write("Invalid email address!: ");
                        userEmail = Console.ReadLine();
                    }
                    Console.WriteLine("Great! Please proceed...");

                    Console.WriteLine();
                    Console.Write("Enter your preferred username: ");
                    string userUsername = Console.ReadLine();

                    Console.WriteLine();
                    Console.WriteLine("Create a password. Your password must be: \r\n1. At least one lower case letter,\r\n2. At least one upper case letter,\r\n3. At least one special character: " + specialCharacters + "\r\n4. At least one number,\r\n5. At least " + minPasswordLength + " characters length.");
                    string newPassword = "";
                    do
                    {
                        newPassword = Console.ReadLine();
                    }
                    while (!validatePassword(newPassword));
                    Console.Write("Valid Password! ");

                    // Confirm Password & Set Limit
                    Console.Write("Input your new password again to confirm it:  ");
                    string confirmPassword = Console.ReadLine();
                    int maxPasswordAttempt = 3;
                    int currentPasswordAttempt = 1;

                    while (true)
                    {
                        if (currentPasswordAttempt < maxPasswordAttempt && confirmPassword != newPassword)
                        {
                            if (currentPasswordAttempt == (maxPasswordAttempt - 1))
                            {
                                Console.Write("Password do not match. You have one attempt left: ");
                            }
                            else
                            {
                                Console.Write("Password do not match. Try again: ");
                            }
                            confirmPassword = Console.ReadLine();
                            currentPasswordAttempt++;
                        }
                        else if (currentPasswordAttempt == maxPasswordAttempt)
                        {
                            Console.WriteLine("Account Setup failed. Attempts exceeded!");
                            break;
                        }

                        if (confirmPassword == newPassword)
                        {
                            Console.WriteLine("Password correct. Account creation successful!");
                            break;
                        }
                    }
                    break;

                    // add menstrual cycle data as procedure//
                }
                else if (welcomeUserResponse == signIn)
                {
                    Console.WriteLine();
                    Console.Write("Kindly sign in using your email address or username: ");
                    string userloginPreference = Console.ReadLine();

                    Console.WriteLine();
                    Console.Write("Enter your password to login:");
                    string userPassword = Console.ReadLine();
                    // if (userPassword != userNewPassword)
                }
                else if (welcomeUserResponse != signUp && welcomeUserResponse != signIn)
                {
                    Console.WriteLine("Invalid Input. Please select fron the options below:\n\t 1. SIGN UP (New Users) \t 2. SIGN IN (Existing Users)");
                    welcomeUserResponse = int.Parse(Console.ReadLine());
                }
            }
        }


        static string readString(string prompt)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();
            } while (result == "");
            return result;
        }


        static int readInt(string prompt)
        {
            int result;

            string intString = readString(prompt);
            while (!int.TryParse(intString, out result))
            {
                intString = readString("Invalid integer input! Please enter a valid integer: ");
            }

            return result;
        }

        static Dictionary<int, string> monthsDictionary = new Dictionary<int, string>();
        static void initializeDictionary()
        {
            monthsDictionary.Add(1, "January");
            monthsDictionary.Add(2, "February");
            monthsDictionary.Add(3, "March");
            monthsDictionary.Add(4, "April");
            monthsDictionary.Add(5, "May");
            monthsDictionary.Add(6, "June");
            monthsDictionary.Add(7, "July");
            monthsDictionary.Add(8, "August");
            monthsDictionary.Add(9, "September");
            monthsDictionary.Add(10, "October");
            monthsDictionary.Add(11, "November");
            monthsDictionary.Add(12, "December");
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Feminence, your personal period tracker.");
            Console.WriteLine(" ...tap the spacebar or enter key to continue...");
            Console.ReadKey();

            Console.WriteLine();
            appStartup();

            initializeDictionary();

            int[] pastPeriodLengths = new int[6];
            DateTime[] startDates = new DateTime[6];


            for (int i = 0; i < pastPeriodLengths.Length; i++)
            {
                pastPeriodLengths[i] = readInt("Enter length for " + monthsDictionary[DateTime.Today.AddMonths(-(i + 1)).Month] + ": ");
                startDates[i] = readDate("Enter your start date");

            }

            Console.ReadKey();
        }
    }
}



