﻿// --------------------------------
// <copyright file="RandomPassword.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AspadLandFramework
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// This class can generate random passwords, which do not include ambiguous 
    /// characters, such as I, l, and 1. The generated password will be made of
    /// 7-bit ASCII symbols. Every four characters will include one lower case
    /// character, one upper case character, one number, and one special symbol
    /// (such as '%') in a random order. The password will always start with an
    /// alpha-numeric character; it will not start with a special symbol (we do
    /// this because some back-end systems do not like certain special
    /// characters in the first position).
    /// </summary>
    public static class RandomPassword
    {
        /// <summary>Minimum password length</summary>
        private static int defaultMinPasswordLength = 8;

        /// <summary>Maximum password length</summary>
        private static int defaultMaxPasswordLength = 10;

        /// <summary>Lowercase characters</summary>
        private static string passwordCharsLowercase = "abcdefgijkmnopqrstwxyz";

        /// <summary>Uppercase characters</summary>
        private static string passwordCharsUppercase = "ABCDEFGHJKLMNPQRSTWXYZ";

        /// <summary>Numeric characters</summary>
        private static string passwordCharsNumeric = "23456789";

        /// <summary>Special characters</summary>
        private static string passwordCharsSpecial = "*$-+?_&=!%{}/";

        /// <summary>Generates a random password.</summary>
        /// <returns>Randomly generated password.</returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random. It will be no shorter than the minimum default and
        /// no longer than maximum default.
        /// </remarks>
        public static string Generate()
        {
            return Generate(defaultMinPasswordLength, defaultMaxPasswordLength);
        }

        /// <summary>
        /// Generates a random password of the exact length.
        /// </summary>
        /// <param name="length">
        /// Exact password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        public static string Generate(int length)
        {
            return Generate(length, length);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <param name="minLength">
        /// Minimum password length.
        /// </param>
        /// <param name="maxLength">
        /// Maximum password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random and it will fall with the range determined by the
        /// function parameters.
        /// </remarks>
        public static string Generate(int minLength, int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
            {
                return null;
            }

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            var charGroups = new char[][] 
            {
                passwordCharsLowercase.ToCharArray(),
                passwordCharsUppercase.ToCharArray(),
                passwordCharsNumeric.ToCharArray(),
                passwordCharsSpecial.ToCharArray()
            };

            // Use this array to track the number of unused characters in each
            // character group.
            var charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
            {
                charsLeftInGroup[i] = charGroups[i].Length;
            }

            // Use this array to track (iterate through) unused character groups.
            var leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
            {
                leftGroupsOrder[i] = i;
            }

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            var randomBytes = new byte[4];

            // Generate 4 random bytes.
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = BitConverter.ToInt32(randomBytes, 0);

            // Now, this is real randomization.
            //Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
            {
                password = new char[new Random(seed).Next(minLength, maxLength + 1)];
            }
            else
            {
                password = new char[minLength];
            }

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                {
                    nextLeftGroupsOrderIdx = 0;
                }
                else
                {
                    nextLeftGroupsOrderIdx = new Random(seed).Next(0, lastLeftGroupsOrderIdx);
                }

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                {
                    nextCharIdx = 0;
                }
                else
                {
                    nextCharIdx = new Random(seed).Next(0, lastCharIdx + 1);
                }

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                {
                    charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                }
                else
                {
                    // There are more unprocessed characters left.
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }

                    // Decrement the number of unprocessed characters in this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                {
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                }
                else
                {
                    // There are more unprocessed groups left.
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }

                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password).Replace("{","S").Replace("}","Z").Replace("[","C").Replace("]","D").Replace("'","1").Replace("\"","2");
        }
    }
}