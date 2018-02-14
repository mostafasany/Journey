using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Models.Post;
using Journey.Services.Buisness.Measurment.Dto;

namespace Journey.Services.Buisness.Measurment.Translators
{
    public static class AccountMeasurmentDataTranslator
    {
        public static AzureAccountMeasurements TranslateAccountMeasurments(string account,
            List<ScaleMeasurment> accountMeasurments)
        {
            try
            {
                var accountMeasurmentsDto = new AzureAccountMeasurements();
                if (accountMeasurments != null)
                {
                    accountMeasurmentsDto.Account = account;
                    accountMeasurmentsDto.Weight = accountMeasurments
                        .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Weight)).Measure;
                    accountMeasurmentsDto.Fat = accountMeasurments
                        .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Fat)).Measure;
                    accountMeasurmentsDto.Water = accountMeasurments
                        .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Water)).Measure;
                    accountMeasurmentsDto.BMI = accountMeasurments
                        .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.BMI)).Measure;
                    accountMeasurmentsDto.Muscle = accountMeasurments
                        .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Muscle)).Measure;
                    accountMeasurmentsDto.BMR = accountMeasurments
                        .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.BMR)).Measure;
                    accountMeasurmentsDto.Protein = accountMeasurments
                        .FirstOrDefault(a => a.Title == nameof(accountMeasurmentsDto.Protein))?.Measure;
                }
                return accountMeasurmentsDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Measurments", ex.InnerException);
            }
        }

        public static List<ScaleMeasurment> TranslateAccountMeasurments(string account,
            AzureAccountMeasurements accountMeasurments)
        {
            try
            {
                var measuremnts = new List<ScaleMeasurment>();
                if (accountMeasurments != null)
                {
                    measuremnts.Add(new ScaleMeasurment
                    {
                        UpIndictor = true,
                        Title = nameof(accountMeasurments.Weight),
                        Unit = "KG",
                        Measure = accountMeasurments.Weight,
                        Image = new Media {Path = "http://bit.ly/2z9lPaA"}
                    });
                    measuremnts.Add(new ScaleMeasurment
                    {
                        UpIndictor = true,
                        Title = nameof(accountMeasurments.Muscle),
                        Unit = "%",
                        Measure = accountMeasurments.Muscle,
                        Image = new Media {Path = "http://bit.ly/2gEeJ2t"}
                    });
                    measuremnts.Add(new ScaleMeasurment
                    {
                        UpIndictor = false,
                        Title = nameof(accountMeasurments.Fat),
                        Unit = "%",
                        Measure = accountMeasurments.Fat,
                        Image = new Media {Path = "http://bit.ly/2xnWzbZ"}
                    });
                    measuremnts.Add(new ScaleMeasurment
                    {
                        UpIndictor = false,
                        Title = nameof(accountMeasurments.BMI),
                        Unit = "",
                        Measure = accountMeasurments.BMI,
                        Image = new Media {Path = "http://bit.ly/2yRyMER"}
                    });
                    measuremnts.Add(new ScaleMeasurment
                    {
                        UpIndictor = true,
                        Title = nameof(accountMeasurments.BMR),
                        Unit = "Kcal",
                        Measure = accountMeasurments.BMR,
                        Image = new Media {Path = "http://bit.ly/2yRzwd7"}
                    });
                    measuremnts.Add(new ScaleMeasurment
                    {
                        UpIndictor = true,
                        Title = nameof(accountMeasurments.Water),
                        Unit = "%",
                        Measure = accountMeasurments.Water,
                        Image = new Media {Path = "http://bit.ly/2yR2J9j"}
                    });
                }
                return measuremnts;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Measurments", ex.InnerException);
            }
        }
    }
}