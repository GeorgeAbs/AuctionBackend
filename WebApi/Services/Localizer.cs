using WebApi.Interfaces;
using static Domain.CoreEnums.Enums;
using static WebApi.Constants.Enums;

namespace WebApi.Services
{
    public class Localizer : ILocalizer
    {
        private Locale _locale;

        public void Initialize(Locale local) => _locale = local;

        public string LocalizeSortings(SortingMethods sorting)
        {
            string result = string.Empty;
            switch (sorting)
            {
                case SortingMethods.PriceAsc:
                    switch(_locale)
                    {
                        case Locale.Ru:
                            result = "По возрастанию цены"; break;
                    }
                    break;

                case SortingMethods.PriceDesc:
                    switch(_locale)
                    {
                        case Locale.Ru:
                            result = "По убыванию цены"; break;
                    }
                    break;

                case SortingMethods.DateAsc:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "Сначала старые"; break;
                    }
                    break;

                case SortingMethods.DateDesc:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "Сначала новые"; break;
                    }
                    break;
            }

            return result;
        }

        public string LocalizeItemTradingStatus(ItemTradingStatus status)
        {
            string result = string.Empty;
            switch (status)
            {
                case ItemTradingStatus.AllStatuses:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "Все позиции"; break;
                    }
                    break;

                case ItemTradingStatus.Template:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "В виде шаблона"; break;
                    }
                    break;

                case ItemTradingStatus.Moderation:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "На модерации"; break;
                    }
                    break;

                case ItemTradingStatus.DisapprovedByModerator:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "Отклонено модератором"; break;
                    }
                    break;

                case ItemTradingStatus.Published:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "Опубликовано"; break;
                    }
                    break;
            }

            return result;
        }

        public string LocalizeReviewCommentType(ReviewCommentsTypes type)
        {
            string result = string.Empty;
            switch (type)
            {
                case ReviewCommentsTypes.My:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "Мои"; break;
                    }
                    break;

                case ReviewCommentsTypes.OnMe:
                    switch (_locale)
                    {
                        case Locale.Ru:
                            result = "Мне"; break;
                    }
                    break;
            }

            return result;
        }
    }
}
