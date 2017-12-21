using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ErrorLoggerModel;
using System.Data.Entity.Validation;

namespace LoadersAndLogic
{
    public class UsersDataHandler
    {
        private static List<User> userData;

        public UsersDataHandler()
        {

            using (ErrorModel db = new ErrorModel())
            {
                db.Database.Initialize(false);
                db.Applications.Count();
            }

            using (ErrorModel context = new ErrorModel())
            {
                userData = context.Users.Include(m => m.Applications).ToList();
                userData = context.Users.Include(m => m.userType).ToList();
            }
        }

        public bool AddUser(User newUser)
        {
            bool result = false;

            using (ErrorModel context = new ErrorModel())
            {
                User UserToAdd = new User()
                {
                    firstName = newUser.firstName,
                    lastName = newUser.lastName,
                    emailID = newUser.emailID,
                    activeStatus = "active",
                    lastLoginDate = DateTime.Now
                };

                UserToAdd.userType = context.UserTypes.First(x => x.userTypeID == 2);

                context.Users.Add(UserToAdd);
                context.SaveChanges();

                result = true;
            }

            return result;
        }

        public bool updateLoginDate(string emailID)
        {
            bool result = false;

            using (ErrorModel context = new ErrorModel())
            {

                var entry = context.Users.SingleOrDefault(u => u.emailID == emailID);
                if (entry != null)
                {
                    try
                    {
                        entry.lastLoginDate = DateTime.Now;
                        entry.userType = entry.userType;
                        context.SaveChanges();

                        result = true;
                    }

                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string property = validationError.PropertyName;
                                string msg = validationError.ErrorMessage;
                                //System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }

            return result;

        }

        public ICollection<User> GetAllUsers()
        {
            return userData;
        }

        public User GetUserByName(string name)
        {
            User user = null;

            if (userData.Any(x => x.firstName == name))
            {
                user = userData.Single(x => x.firstName == name);
            }

            return user;
        }

        public List<int> GetUserApps(string mailID)
        {
            User user = null;
            List<int> appIDs = null;

            user = userData.Single(x => x.emailID == mailID);
            appIDs = user.Applications.Select(y => y.appId).ToList();

            return appIDs;
        }

    }
}
