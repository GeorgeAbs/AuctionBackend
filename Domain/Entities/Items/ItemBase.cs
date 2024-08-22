using Domain.Attribytes;
using Microsoft.EntityFrameworkCore;
using static Domain.CoreEnums.Enums;

namespace Domain.Entities.Items
{
    public abstract class ItemBase : EntityBase
    {
        [ItemPropertyValidationTypeAttribyte(ItemPropertyValidationType.ByRulesForUserTypedText)]
        public string Title { get; set; }

        [ItemPropertyValidationTypeAttribyte(ItemPropertyValidationType.ByRulesForUserTypedText)]
        public string Description { get; set; }

        public Guid UserId { get; private set; }

        public Guid CustomerId { get; private set; }// I think it is not suitable here, need move to orders

        public DateTime StatusChangingLastTime { get; set; } = DateTime.UtcNow;

        protected bool _isPromotedByPriority;
        [BackingField(nameof(_isPromotedByPriority))]
        public bool IsPromotedByPriority { get { return _isPromotedByPriority; } private set { _isPromotedByPriority = value; } }

        protected DateTime _promotionByPriorityStartTime;
        [BackingField(nameof(_promotionByPriorityStartTime))]
        public DateTime PromotionByPriorityStartTime { get { return _promotionByPriorityStartTime; } private set { _promotionByPriorityStartTime = value; } }

        protected DesignPromotionType _designPromotionType;
        [BackingField(nameof(_designPromotionType))]
        public DesignPromotionType DesignPromotionType { get { return _designPromotionType; } private set { _designPromotionType = value; } }

        protected DateTime _promotionByDesignStartTime;
        [BackingField(nameof(_promotionByDesignStartTime))]
        public DateTime PromotionByDesignStartTime { get { return _promotionByDesignStartTime; } private set { _promotionByDesignStartTime = value; } }

        public ItemBase() { }

        public ItemBase(Guid userId,
            string title,
            string description)
        {
            UserId = userId;
            Title = title;
            Description = description;
            _isPromotedByPriority = false;
            _designPromotionType = DesignPromotionType.No;

        }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
        }

        public void SetPromotionByDesign(DesignPromotionType designPromotionType = DesignPromotionType.No)
        {
            _designPromotionType = designPromotionType;
            _promotionByDesignStartTime = DateTime.UtcNow;
        }

        public void EnablePromotionByPriority(bool isEnable)
        {
            if (isEnable)
            {
                _isPromotedByPriority = true;
                _promotionByPriorityStartTime = DateTime.UtcNow;
            }

            else
            {
                _isPromotedByPriority = false;
            }
        }
    }
}