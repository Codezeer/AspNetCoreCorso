using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyCourse.Customizations.TagHelpers
{
    public class RatingTagHelper:TagHelper
    {
        //(1) In questo modo viene settato automaticamente dal modelbinder l'attributo value context.AllAttributes["value"]
        public double Value { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //double value = (double)context.AllAttributes["value"].Value;
            for (int i = 1; i <= 5; i++)
        {
            if(Value >= i)
            {
                output.Content.AppendHtml("<i class=\"fas fa-star\"></i>");
            }
            else if(Value > i -1)
            {
                output.Content.AppendHtml("<i class=\"fas fa-star-half-alt\"></i>");
            }
            else
            {
                output.Content.AppendHtml("<i class=\"far fa-star\"></i>");
            }
        }
        }
    }
}