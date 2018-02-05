using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;
using Abstractions.Models;
using Journey.Services.Buisness.Account.Entity;

namespace Journey.Services.Buisness.Account.Translators
{
    public static class AccountDataTranslator
    {
        #region Transaltors

        public static AzureAccount TranslateAccount(Tawasol.Models.Account account)
        {
            try
            {
                var accountDto = new AzureAccount();
                if (account == null) return accountDto;
                if (!string.IsNullOrEmpty(account.Id))
                    accountDto.Id = account.Id;
                accountDto.FName = account.FirstName;
                accountDto.LName = account.LastName;
                accountDto.Profile = account.Image?.Path;
                accountDto.SToken = account.SocialToken;
                accountDto.SProvider = account.SocialProvider;
                accountDto.SID = account.SID;
                accountDto.Email = account.Email;
                accountDto.Gender = account.Gender;
                accountDto.Status = account.Status;
                accountDto.Challenge = account.ChallengeId;
                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Account", ex.InnerException);
            }
        }

        public static List<Tawasol.Models.Account> TranslateAccounts(List<AzureAccount> accounts)
        {
            try
            {
                return accounts.Select(TranslateAccount).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Account", ex.InnerException);
            }
        }

        public static Tawasol.Models.Account TranslateAccount(AzureAccount account)
        {
            try
            {
                var accountDto = new Tawasol.Models.Account();
                if (account == null) return accountDto;
                if (!string.IsNullOrEmpty(account.Id))
                    accountDto.Id = account.Id;
                accountDto.FirstName = account.FName;
                accountDto.LastName = account.LName;
                accountDto.Image = new Media {Path = account.Profile};
                accountDto.SocialToken = account.SToken;
                accountDto.SocialProvider = account.SProvider;
                accountDto.SID = account.SID;
                accountDto.Email = account.Email;
                accountDto.Gender = account.Gender;
                accountDto.Status = account.Status;
                accountDto.ChallengeId = account.Challenge;
                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Account", ex.InnerException);
            }
        }

        //public static AzureAccountMeasurements TranslateAccountMeasurments(string account,
        //    List<ScaleMeasurment> accountMeasurments)
        //{
        //    try
        //    {
        //        AzureAccountMeasurements accountMeasurmentsDto = new AzureAccountMeasurements();
        //        if (accountMeasurments != null)
        //        {
        //            accountMeasurmentsDto.Account = account;
        //            accountMeasurmentsDto.Weight = accountMeasurments
        //                .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Weight)).Measure;
        //            accountMeasurmentsDto.Fat = accountMeasurments
        //                .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Fat)).Measure;
        //            accountMeasurmentsDto.Water = accountMeasurments
        //                .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Water)).Measure;
        //            accountMeasurmentsDto.BMI = accountMeasurments
        //                .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.BMI)).Measure;
        //            accountMeasurmentsDto.Muscle = accountMeasurments
        //                .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Muscle)).Measure;
        //            accountMeasurmentsDto.BMR = accountMeasurments
        //                .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.BMR)).Measure;
        //            accountMeasurmentsDto.Protein = accountMeasurments
        //                .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Protein))?.Measure;
        //        }
        //        return accountMeasurmentsDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionService.Handle(ex);
        //        return null;
        //    }
        //}

        //public static List<ScaleMeasurment> TranslateAccountMeasurments(string account,
        //    AzureAccountMeasurements accountMeasurments)
        //{
        //    try
        //    {
        //        List<ScaleMeasurment> measuremnts = new List<ScaleMeasurment>();
        //        if (accountMeasurments != null)
        //        {
        //            measuremnts.Add(new ScaleMeasurment
        //            {
        //                UpIndictor = true,
        //                Title = nameof(accountMeasurments.Weight),
        //                Unit = "KG",
        //                Measure = accountMeasurments.Weight,
        //                Image = new Models.Image {Path = "http://bit.ly/2z9lPaA"}
        //            });
        //            measuremnts.Add(new ScaleMeasurment
        //            {
        //                UpIndictor = true,
        //                Title = nameof(accountMeasurments.Muscle),
        //                Unit = "%",
        //                Measure = accountMeasurments.Muscle,
        //                Image = new Models.Image {Path = "http://bit.ly/2gEeJ2t"}
        //            });
        //            measuremnts.Add(new ScaleMeasurment
        //            {
        //                UpIndictor = false,
        //                Title = nameof(accountMeasurments.Fat),
        //                Unit = "%",
        //                Measure = accountMeasurments.Fat,
        //                Image = new Models.Image {Path = "http://bit.ly/2xnWzbZ"}
        //            });
        //            measuremnts.Add(new ScaleMeasurment
        //            {
        //                UpIndictor = false,
        //                Title = nameof(accountMeasurments.BMI),
        //                Unit = "",
        //                Measure = accountMeasurments.BMI,
        //                Image = new Models.Image {Path = "http://bit.ly/2yRyMER"}
        //            });
        //            measuremnts.Add(new ScaleMeasurment
        //            {
        //                UpIndictor = true,
        //                Title = nameof(accountMeasurments.BMR),
        //                Unit = "Kcal",
        //                Measure = accountMeasurments.BMR,
        //                Image = new Models.Image {Path = "http://bit.ly/2yRzwd7"}
        //            });
        //            measuremnts.Add(new ScaleMeasurment
        //            {
        //                UpIndictor = true,
        //                Title = nameof(accountMeasurments.Water),
        //                Unit = "%",
        //                Measure = accountMeasurments.Water,
        //                Image = new Models.Image {Path = "http://bit.ly/2yR2J9j"}
        //            });
        //        }
        //        return measuremnts;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionService.Handle(ex);
        //        return null;
        //    }
        //}

        //public static AccountGoal TranslateAccountGoal(AzureAccountGoal accountGoal)
        //{
        //    try
        //    {
        //        var accountDto = new AccountGoal();
        //        if (accountGoal != null)
        //        {
        //            accountDto.Weight = accountGoal.Weight;
        //            accountDto.Goal = accountGoal.Goal;
        //            accountDto.Start = accountGoal.Start;
        //            accountDto.End = accountGoal.End;
        //        }

        //        return accountDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionService.Handle(ex);
        //        return null;
        //    }
        //}


        //public static AzureAccountGoal TranslateAccountGoal(AccountGoal accountGoal, string account)
        //{
        //    try
        //    {
        //        AzureAccountGoal accountDto = new AzureAccountGoal();
        //        if (accountGoal != null)
        //        {
        //            accountDto.Account = account;
        //            accountDto.Weight = accountGoal.Weight;
        //            accountDto.Goal = accountGoal.Goal;
        //            accountDto.Start = accountGoal.Start;
        //            accountDto.End = accountGoal.End;
        //        }
        //        return accountDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionService.Handle(ex);
        //        return null;
        //    }
        //}

        #endregion
    }
}