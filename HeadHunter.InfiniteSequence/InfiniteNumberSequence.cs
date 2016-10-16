using System;
using System.Numerics;
using System.Text;

namespace HeadHunter.InfiniteSequence
{
    static class InfiniteNumberSequence
    {
        public static BigInteger IndexOfFragment(string fragment)
        {
            var fragmentLength = fragment.Length;

            for (var numberLength = 1; numberLength <= fragmentLength; numberLength++)
            {
                for (var numberOffset = 0; numberOffset < numberLength; numberOffset++)
                {
                    if(fragment[numberOffset] == '0')
                    {
                        // number cannot be started from here
                        continue;
                    }

                    var assumptionNumber = GetAssumptionNumber(numberLength, numberOffset, fragment);
                    var assumptionFragment = MakeFragment(assumptionNumber, numberOffset, fragmentLength);

                    if(fragment == assumptionFragment)
                    {
                        var indexOfNumber = IndexOfNumber(assumptionNumber);
                        return indexOfNumber - numberOffset;
                    }
                }
            }

            // if we here that fragment must contains only zeros
            var fragmentNumber = BigInteger.Parse("1" + fragment); // shoud be equal to 10 ^ fragment.Lenght
            return IndexOfNumber(fragmentNumber) + 1;
        }

        /// <summary>
        /// Method does assumption that fragment contains number with numberLength digits that starts from numberOffset index in fragment.
        /// </summary>
        /// <param name="numberLength">Count digits in number</param>
        /// <param name="numberOffset">Index in fragment that a number starts from</param>
        /// <param name="fragment">Fragment of infinite sequence of numbers</param>
        /// <returns>Number that was retrived from fragment</returns>
        private static BigInteger GetAssumptionNumber(int numberLength, int numberOffset, string fragment)
        {
            if(numberOffset < 0)
            {
                throw new ArgumentOutOfRangeException("numberOffset", numberOffset, "It's not univeral method. NumberOffset must be non-negative.");
            }
            if(numberOffset >= numberLength)
            {
                throw new ArgumentOutOfRangeException("numberOffset", numberOffset, "It's not univeral method. NumberOffset must be less then numberLength.");
            }
            if (numberOffset >= fragment.Length)
            {
                throw new ArgumentOutOfRangeException("numberOffset", numberOffset, "It's not univeral method. NumberOffset must be less then fragment length.");
            }

            var digitCountBeyondFragment = numberOffset + numberLength - fragment.Length;
            if (digitCountBeyondFragment <= 0)
            {
                var strNumber = fragment.Substring(numberOffset, numberLength);
                return BigInteger.Parse(strNumber);
            }

            // here we have fragment that consist of two parts of two sequence numbers {firstNumberRightPart}{secondNumberLeftPart}

            var firstNumberRightPartStr = fragment.Substring(0, numberOffset);
            var firstNumberRightPart = BigInteger.Parse(firstNumberRightPartStr);

            var secondNumberLeftPartStr = fragment.Substring(numberOffset);
            var secondNumberRightPartRequiredLength = numberLength - secondNumberLeftPartStr.Length;

            var secondNumberRightPartStr = (firstNumberRightPart + 1).ToString();
            if(secondNumberRightPartStr.Length > secondNumberRightPartRequiredLength)
            {
                // we can be here if firstNumberRightPart consists of only digits '9'
                secondNumberRightPartStr = secondNumberRightPartStr.Substring(secondNumberRightPartRequiredLength);
            }
            else if(secondNumberRightPartStr.Length < secondNumberRightPartRequiredLength)
            {
                // we can be here if firstNumberRightPart starts from digits '0'
                var secondNumberRightPartLackLength = secondNumberRightPartRequiredLength - secondNumberRightPartStr.Length;
                secondNumberRightPartStr = new string('0', secondNumberRightPartLackLength) + secondNumberRightPartStr;
            }

            return BigInteger.Parse(secondNumberLeftPartStr + secondNumberRightPartStr);
        }

        /// <summary>
        /// Make a fragment of infinite number sequence.
        /// </summary>
        /// <param name="value">Number that a resulting fragment contains</param>
        /// <param name="offset">Index of a number in resulting fragment</param>
        /// <param name="requiredFragmentLength">Count of digits that resulting fragment needs to consist of</param>
        /// <returns>Fragment of infinite sequence of numbers</returns>
        private static string MakeFragment(BigInteger number, int numberOffset, int requiredFragmentLength)
        {
            var numberLength = number.ToString().Length;

            if (numberOffset < 0)
            {
                throw new ArgumentOutOfRangeException("numberOffset", numberOffset, "It's not univeral method. NumberOffset must be non-negative.");
            }
            if (numberOffset >= numberLength)
            {
                throw new ArgumentOutOfRangeException("numberOffset", numberOffset, "It's not univeral method. NumberOffset must be less then number length.");
            }

            var fragment = new StringBuilder();
            if (numberOffset > 0)
            {
                // add right part of a first number of an infinite sequence fragment
                var previousNumberStr = (number - 1).ToString();
                fragment.Append(previousNumberStr.Right(numberOffset));
            }

            var nextNumber = number;
            while (fragment.Length < requiredFragmentLength)
            {
                fragment.Append(nextNumber);
                nextNumber++;
            }

            return fragment.ToString(0, requiredFragmentLength);
        }

        public static BigInteger IndexOfNumber(BigInteger number)
        {
            if(number < 10)
            {
                return number;
            }

            var searchedValueDigitCount = number.ToString().Length;
            var searchedValueIndex = new BigInteger(0);

            for(var digitCount = 1; digitCount < searchedValueDigitCount; digitCount++)
            {
                // add to searchedValueIndex the count of digit for all number with digitCount digits in it
                searchedValueIndex += digitCount * 9 * new BigInteger(Math.Pow(10, digitCount - 1));
            }

            // add to searchedValueIndex the count of digit for numbers with digitCount digits in it that stands before searchedValue
            searchedValueIndex += (number % 10) * searchedValueDigitCount;

            // before that we calculated all digits before searchedValue, so we need increment index by one
            searchedValueIndex++;

            return searchedValueIndex;
        }
    }
}
