using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Abstractions.Exceptions;
using Journey.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journey.Resources
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                if (Text == null)
                    return null;
                var resourceManager = new ResourceManager(Constant.ResourceId,
                    typeof(TranslateExtension).GetTypeInfo().Assembly);

                return resourceManager.GetString(Text, CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message, ex);
            }
        }
    }
}