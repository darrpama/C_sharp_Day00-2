double GetTotalMonthlyPayment(double loanAmmount, double interestRateOnLoanPerMounth, int mounths) {
  if (mounths < 0) {
    throw new ArgumentException("Number of mounths should be > 0");
  }
  double montlyPayment = 0;
  montlyPayment = (loanAmmount * interestRateOnLoanPerMounth *
                   Math.Pow((1 + interestRateOnLoanPerMounth), mounths)) /
                  (Math.Pow((1 + interestRateOnLoanPerMounth), mounths) - 1);
  return montlyPayment;
}

double GetInterestRateOnLoanPerMounth(double annualPercentageRate) {
  double interestRateOnLoanPerMounth = 0.0;
  interestRateOnLoanPerMounth = annualPercentageRate / 12 / 100;
  return interestRateOnLoanPerMounth;
}

double GetMonthlyPaymentInterest(double totalDebtBalance, double annualPercentageRate,
                                 int daysOfPeriod, int daysPerYear) {
  double monthlyPaymentInterest = 0.0;
  monthlyPaymentInterest =
      (totalDebtBalance * annualPercentageRate * daysOfPeriod) / (100 * daysPerYear);
  return monthlyPaymentInterest;
}

void PrintOutput(int mounths, double sum, double rate) {
  DateTime currentDate = DateTime.Now;
  DateTime paymentDate = currentDate.AddDays(-currentDate.Day + 1);

  double ratePerMonth = GetInterestRateOnLoanPerMounth(rate);
  double monthlyPayment = GetTotalMonthlyPayment(sum, ratePerMonth, mounths);

  for (int i = 1; i <= mounths; i++) {
    int daysOfThePeriod = DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month);
    int daysOfTheYear = DateTime.IsLeapYear(paymentDate.Year) ? 366 : 365;
    double monthlyPaymentInterest =
        GetMonthlyPaymentInterest(sum, rate, daysOfThePeriod, daysOfTheYear);
    double principalDebt = monthlyPayment - monthlyPaymentInterest;
    double remainingDebt = sum - principalDebt;
    if (remainingDebt < 0) {
      remainingDebt = 0;
    }
    if (i == mounths) {
      monthlyPayment += remainingDebt;
      principalDebt += remainingDebt;
      remainingDebt = 0;
    }
    paymentDate = paymentDate.AddMonths(1);
    Console.WriteLine("{0}\t{1}\t{2:n2}\t{3:n2}\t{4:n2}\t{5:n2}", i,
                      paymentDate.ToString("MM/dd/yyyy"), monthlyPayment, principalDebt,
                      monthlyPaymentInterest, remainingDebt);

    sum -= principalDebt;
  }
}

try {
  if (args.Length != 3) {
    throw new ArgumentException("Something went wrong. Check your input and retry.");
  }

  double loanAmount = Convert.ToDouble(args[0]);
  double annualPercentageRate = Convert.ToDouble(args[1]);
  int numberOfMonths = Convert.ToInt32(args[2]);

  PrintOutput(numberOfMonths, loanAmount, annualPercentageRate);
} catch (Exception e) {
  Console.WriteLine(e.Message);
}
