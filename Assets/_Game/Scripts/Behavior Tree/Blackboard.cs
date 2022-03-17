using System.Collections.Generic;
using UnityEngine;

namespace Alchemist.AI
{
    /// <summary>
    /// Blackboard holds general information to be used in Behaviour tree.
    /// </summary>
    public class Blackboard
    {
        private readonly Dictionary<string, object> _database = new Dictionary<string, object>();

        /// <summary>
        /// Adds new data to blackboard.
        /// </summary>
        /// <param name="informationName">Information name to add</param>
        /// <param name="information">Information to add</param>
        /// <typeparam name="T">Information type</typeparam>
        public void Add<T>(string informationName, T information)
        {
            _database.Add(informationName, information);
        }

        /// <summary>
        /// Returns information from database.
        /// </summary>
        /// <param name="informationName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string informationName)
        {
            return (T) _database[informationName];
        }

        /// <summary>
        /// Removes information from database.
        /// </summary>
        /// <param name="informationName">Information to remove</param>
        public void Remove(string informationName)
        {
            _database.Remove(informationName);
        }

        /// <summary>
        /// Update information with given value.
        /// </summary>
        /// <param name="informationName">Information to edit</param>
        /// <param name="information">New information</param>
        /// <typeparam name="T">Information Type</typeparam>
        public void Update<T>(string informationName, T information)
        {
            _database[informationName] = information;
        }
    }
}