using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace EthosLending_EmreTurul
{
    class Program
    {
        static double Amount = 0;
        static double Interest = 0;
        static double DownPayment = 0;
        static double Term = 0;

        static double MonthlyPayment = 0;
        static double TotalInterest = 0;
        static double TotalPayment = 0;

        static void Main(string[] args)
        {

            try
            {
                // reading input values
                ReadInputVariables();

                // Calculate the loan payment
                CalculateLoan();


                // Generate JSON
                GenerateJSON();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.ReadKey();
            }

        }

        static void ReadInputVariables(){

            string inputText = Console.ReadLine();

            string AmountText = getDigitPart(InputType.Amount, inputText);
            Amount = convertToDouble(AmountText);


            inputText = Console.ReadLine();

            string InterestText = getDigitPart(InputType.Interest, inputText).Replace("%", "");
            Interest = convertToDouble(InterestText);


            inputText = Console.ReadLine();

            string DownPaymentText = getDigitPart(InputType.DownPayment, inputText);
            DownPayment = convertToDouble(DownPaymentText);


            inputText = Console.ReadLine();

            string TermText = getDigitPart(InputType.Term, inputText);
            Term = convertToDouble(TermText);


            Console.ReadLine();

        }

        static void CalculateLoan(){

            double MonthlyInterestRate = Interest / (100 * 12);
            double NumberOfPayments = Term * 12;


            MonthlyPayment = (MonthlyInterestRate * (Amount - DownPayment)) / (1 - Math.Pow(1 + MonthlyInterestRate, NumberOfPayments * -1));

            TotalPayment = MonthlyPayment * NumberOfPayments;

            TotalInterest = TotalPayment - (Amount - DownPayment);
        }

        static void GenerateJSON(){

            LoanDTO loanDTO = new LoanDTO() { MonthlyPayment = Math.Round(MonthlyPayment, 2), TotalPayment = Math.Round(TotalPayment, 2), TotalInterest = Math.Round(TotalInterest, 2) };

            var output = JsonConvert.SerializeObject(loanDTO);

            Console.WriteLine(output);
            Console.ReadKey();
        }






        static double convertToDouble(string text){
          
            double result;

            if(Double.TryParse(text, out result))
            {
                return result;
            }
            else
            {
                throw new Exception("Wrong input!");
            }

        }

        static string getDigitPart(InputType inputType,string text)
        {
            if(text.ToLower().IndexOf(inputType.ToString().ToLower(), StringComparison.Ordinal) == -1 )
            {
                throw new Exception($"{inputType.ToString()} is not valid!");
            }
            else if (text.IndexOf(Convert.ToChar(":")) == -1)
            {
                throw new Exception($"{inputType.ToString()} is not valid!");
            }


            return text.ToString().Substring(text.IndexOf(Convert.ToChar(":"))+1).Trim();
        }
       


    }

    enum InputType {
        Amount,
        Interest,
        DownPayment,
        Term
    }





}

