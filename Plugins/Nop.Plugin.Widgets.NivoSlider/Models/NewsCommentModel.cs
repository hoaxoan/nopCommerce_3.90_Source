using System;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.NivoSlider.Models
{
    public partial class NewsCommentModel : BaseNopEntityModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAvatarUrl { get; set; }

        public string CommentTitle { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool AllowViewingProfiles { get; set; }
    }
}