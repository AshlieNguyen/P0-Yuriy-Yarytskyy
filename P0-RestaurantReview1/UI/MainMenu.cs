using Models;
using System;
using BL;
using System.Collections.Generic;

namespace UI
{
    public class MainMenu : IMenu
    {
        private IUserBL _userbl;
        public MainMenu(IUserBL bl)
        {
            _userbl = bl;
        }
        public void Start()
        {
        
            bool repeat = true;
            do
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("*  WELCOME TO RESTAURANT REVIEW APP!  *");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("*      PLEASE MAKE YOUR SELECTION     *");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[0] EXIT");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[1] ADD A USER");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[2] WRITE A REVIEW");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[3] SEE RESTAURANT'S RATING");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[4] VIEW ALL REVIEWS");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[5] SEARCH FOR A RESTAURANT");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[6] VIEW ALL RESTAURANTS");
                Console.WriteLine("---------------------------------------");

                switch(Console.ReadLine())
                {
                    case "0":
                        Console.WriteLine("You have chosen to exit!");
                        repeat = false;
                    break;

                    case "1":
                        AddUser();
                    break;

                    case "2":
                        AddReview();
                    break;

                    case "3":
                        GetAvgRating();
                    break;

                    case "4":
                        ViewAllReviews();
                    break;

                    case "5":
                        SearchForARestaurant();
                    break;

                    case "6":
                        ViewAllRestaurants();
                    break;

                    default:
                        Console.WriteLine("WRONG SELECTION!!!");
                    break;
                }
            }while(repeat);

        }

        private void AddUser()
        {
            string inputName;
            string inputPassword;
            string inputEmail;
            User userToAdd;


            Console.WriteLine("PLEASE ENTER NEW USER DRTAILS!");

            do
            {
                Console.WriteLine("Name: ");
                inputName = Console.ReadLine();
                Console.WriteLine("Password: ");
                inputPassword = Console.ReadLine();
                Console.WriteLine("Email: ");
                inputEmail = Console.ReadLine();

            }while(String.IsNullOrWhiteSpace(inputName) && String.IsNullOrWhiteSpace(inputPassword) && String.IsNullOrWhiteSpace(inputEmail));

            userToAdd = new User(inputName, inputPassword, inputEmail);
            userToAdd = _userbl.AddUser(userToAdd);
            Console.WriteLine("_____________________________________________________");
            Console.WriteLine($"{userToAdd.Name} was successfully added as a user.");
            Console.WriteLine("_____________________________________________________");
        }
        private void AddReview()
        {
            decimal rating = 0;
            string comment;
            int userId;
            int restaurantId;
            

            List<User> users = _userbl.ViewAllUsers();
            Console.WriteLine("__________________________________");
            string promptUser = "|  WHO WANTS TO LEAVE A REVIEW?  |";
            User selectedUser = SelectUser(users, promptUser);

            List<Restaurant> restaurants = _userbl.ViewAllRestaurants();
            Console.WriteLine("__________________________________");
            string promptRestaurant = "|  SELECT A RESTAURANT TO REVIEW  |";
            Restaurant selectedRestaurant = SelectRestaurant(restaurants, promptRestaurant);
            
            if(selectedUser is not null)
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine(selectedUser.Name + " wants to write a review");
                Console.WriteLine("----------------------------------");

                     if(selectedRestaurant is not null)
                    {
                        do
                        {
                            Console.WriteLine("You have selected " + selectedRestaurant.Name + " to review");
                            Console.WriteLine("Rate this restaurant 1 to 5: ");
                            rating = Convert.ToDecimal(Console.ReadLine());
                            Console.WriteLine("You have given a score of {0}", rating);
                            Console.WriteLine("Please leave a comment: ");
                            comment = Console.ReadLine();

                        }while(String.IsNullOrWhiteSpace(comment));
                        
                        Console.WriteLine(selectedRestaurant.Id);
                        Console.WriteLine(selectedUser.Id);

                        Review reviewToAdd = new Review( rating, comment, selectedRestaurant.Id, selectedUser.Id);

                        try
                        {
                            reviewToAdd = _userbl.AddReview(reviewToAdd);
                            Console.WriteLine("------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(selectedUser.Name + " has added new review to " + selectedRestaurant.Name + " restaurant successfully.");
                        }
                        catch (Exception ex)
                        {
                            
                            Console.WriteLine(ex);//add loger in parantesis
                        }
                        
                    }

            }

        }
        private void ViewAllReviews()
        {
            List<Review> reviews = _userbl.ViewAllReviews();
            foreach(Review review in reviews)
            {
                Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"    {review.Id}   *   {review.Rating}   *   {review.Comment}   *   ");
                Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
            }
        }
        private void SearchForUser()
        {
            Console.WriteLine("Searching for a user");
        }

        private void SearchForARestaurant()
        {
            bool repeat = true;
            do
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("*          RESTAURANT SEARCH          *");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("*  PLEASE CHOOSE ONE OF THE FOLLOWING  *");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[1] SEARCH BY NAME");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[2] SEARCH BY TYPE");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[3] SEARCH BY RATING");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[4] SEARCH BY CITY");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[5] SEARCH BY ZIPCODE");
                Console.WriteLine("---------------------------------------");;
                Console.WriteLine("[6] View ALL Restaurants");
                Console.WriteLine("---------------------------------------");

                switch(Console.ReadLine())
                {
                    case "0":
                        Console.WriteLine("EXIT OPTION HAS BEEN SELECTED!");
                        repeat = false;
                    break;

                    case "1":
                        SearchRestaurantName();
                    break;

                    case "2":
                        SearchRestaurantType();
                    break;

                    case "3":
                        SearchRestaurantRating();
                    break;

                    case "4":
                        SearchRestaurantCity();
                    break;

                    case "5":
                        SearchRestaurantZipCode();
                    break;

                    case "6":
                        ViewAllRestaurants();
                    break;

                    default:
                        Console.WriteLine("!!! INCORRECT SELLECTION !!!");
                    break;
                }
            }while(repeat);
        }
        private void SearchRestaurantName()
        {
            string input;
            Console.WriteLine("ENTER RESTAURANT'S NAME");
            input = Console.ReadLine();

            Restaurant foundRestaurant = _userbl.SearchRestaurantName(input);
            if(foundRestaurant.Name is null)
            {
                Console.WriteLine($"{input} no such restaurant exists, please try a different entry");
            }
            else
            {
                Console.WriteLine($"FOUND: {foundRestaurant.Name} {foundRestaurant.Address} {foundRestaurant.City} {foundRestaurant.State} {foundRestaurant.ZipCode}");
            }
        }
        private void SearchRestaurantType()
        {
            
            string input;
            Console.WriteLine("ENTER TYPE OF FOOD");
            input = Console.ReadLine();

            List<Restaurant> foundRestaurants = _userbl.ViewAllRestaurants();
            foreach(Restaurant foundRestaurant in foundRestaurants)
            {
                if(foundRestaurant.Type == input)
                {
                     if(foundRestaurant.Type is null)
                    {
                        Console.WriteLine($"{input} no such restaurant exists, please try a different entry");
                    }
                    else
                    {
                        Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine($"    Restaurant ID: {foundRestaurant.Id}      Food-Type: {foundRestaurant.Type}      Name: {foundRestaurant.Name}      Address: {foundRestaurant.Address}, {foundRestaurant.City}, {foundRestaurant.State}, {foundRestaurant.ZipCode}");
                        Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");            
                    }
                }
            }
           
        }

        private void SearchRestaurantRating()
        {
            decimal input;
            Console.WriteLine("ENTER RATING NUMBER YOU WANT TO SEE");
            input = Convert.ToDecimal(Console.ReadLine());

            Review foundReview = _userbl.SearchRestaurantRating(input);
            if(foundReview.Rating == 0)
            {
                Console.WriteLine($"{input} no such restaurant exists, please try a different entry");
            }
            else
            {
                Console.WriteLine("FOUND: {0}", foundReview.Rating );
            }
        }

        private void SearchRestaurantCity()
        {
            string input;
            Console.WriteLine("PLEASE ENTER A CITY FOR YOUR SEARCH");
            input = Console.ReadLine();

            List<Restaurant> foundRestaurants = _userbl.ViewAllRestaurants();
            foreach(Restaurant foundRestaurant in foundRestaurants)
            {
                if(foundRestaurant.City == input)
                {
                     if(foundRestaurant.City is null)
                    {
                        Console.WriteLine($"{input} no such city in our records. Please re-enter.");
                    }
                    else
                    {
                        Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine($"    Restaurant ID: {foundRestaurant.Id}      Food-Type: {foundRestaurant.Type}      Name: {foundRestaurant.Name}      Address: {foundRestaurant.Address}, {foundRestaurant.City}, {foundRestaurant.State}, {foundRestaurant.ZipCode}");
                        Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");            
                    }
                }
            }
        }

        private void SearchRestaurantZipCode()
        {
            int input;
            Console.WriteLine("PLEASE ENTER A ZIPCODE TO SEARCH");
            input = Convert.ToInt32(Console.ReadLine());

            List<Restaurant> foundRestaurants = _userbl.ViewAllRestaurants();
            foreach(Restaurant foundRestaurant in foundRestaurants)
            {
                if(foundRestaurant.ZipCode == input)
                {
                     if(foundRestaurant.ZipCode == 0)
                    {
                        Console.WriteLine($"{input} no such restaurant exists, please try a different entry");
                    }
                    else
                    {
                        Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine($"    Restaurant ID: {foundRestaurant.Id}      Food-Type: {foundRestaurant.Type}      Name: {foundRestaurant.Name}      Address: {foundRestaurant.Address}, {foundRestaurant.City}, {foundRestaurant.State}, {foundRestaurant.ZipCode}");
                        Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");            
                    }
                }
            }
        }
        private void ViewAllRestaurants()
        {
            List<Restaurant> restaurants = _userbl.ViewAllRestaurants();
            foreach(Restaurant restaurant in restaurants)
            {
                Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"    Restaurant ID: {restaurant.Id}      Food-Type: {restaurant.Type}      Name: {restaurant.Name}      Address: {restaurant.Address}, {restaurant.City}, {restaurant.State}, {restaurant.ZipCode}");
                Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
            }
        }

        public Restaurant SelectRestaurant(List<Restaurant> restaurants, string prompt)
        {
            Console.WriteLine(prompt);

            int selection;
            bool valid = false;

            do
            {
                for(int i = 0; i < restaurants.Count; i++)
                {
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"[{i}] {restaurants[i].Name}");
                }

                valid = int.TryParse(Console.ReadLine(), out selection);

                if(valid && (selection >= 0 && selection < restaurants.Count))
                {
                    return restaurants[selection];
                }

                Console.WriteLine("___________________________");
                Console.WriteLine("  Enter valid selection  ");
                Console.WriteLine("___________________________");
            }while(true);
        }

        public User SelectUser(List<User> users, string prompt)
        {
            Console.WriteLine(prompt);

            int selection;
            bool valid = false;

            do
            {
                for(int i = 0; i < users.Count; i++)
                {
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"[{i}] {users[i].Name}");
                }

                valid = int.TryParse(Console.ReadLine(), out selection);

                if(valid && (selection >= 0 && selection < users.Count))
                {
                    return users[selection];
                }

                Console.WriteLine("___________________________");
                Console.WriteLine("  Enter valid selection  ");
                Console.WriteLine("___________________________");
            }while(true);
        }

         private void ViewAllUsers()
        {
            List<Restaurant> restaurants = _userbl.ViewAllRestaurants();
            foreach(Restaurant restaurant in restaurants)
            {
                Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"    Restaurant ID: {restaurant.Id}      Food-Type: {restaurant.Type}      Name: {restaurant.Name}      Address: {restaurant.Address}, {restaurant.City}, {restaurant.State}, {restaurant.ZipCode}");
                Console.WriteLine("  -----------------------------------------------------------------------------------------------------------------------------");
            }
        }

        private Restaurant GetRating(List<Restaurant> restaurants)
        {
            int select;

            bool valid = false;


           Console.WriteLine("Which restaurant you want to get the ratings from?");

            do{
                for( int i=0; i<restaurants.Count; i++)
                {
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"[{i}] {restaurants[i].Name}");

                }

                valid = int.TryParse(Console.ReadLine(), out select);
               if(valid && (select >= 0 && select < restaurants.Count))
                {

                        return restaurants[select];
                }
                Console.WriteLine("PLEASE USE VALID ENTRY");
            }while(true);

        }

        public  void GetAvgRating()
        {

            decimal rating =0;
            decimal div=0;
            decimal average=0;

            List<Restaurant> restaurants = _userbl.ViewAllRestaurants();
            Restaurant operandx = GetRating(restaurants);

            List<Review> reviews = _userbl.ViewAllReviews();


            for( int i = 0; i<reviews.Count; i++)
            {

                if(operandx.Id == reviews[i].RestaurantId)
                {
                    div++;
                    rating = rating + Convert.ToDecimal( reviews[i].Rating);

                }


            }
            average =  rating/div;

            Console.WriteLine("The average of " + operandx.Name + " is: " + average);

        }

    }
}