using System;
using System.ServiceModel.Configuration;

namespace ec.edu.monster.Seguridad
{
    public class TokenHeaderBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(TokenHeaderEndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new TokenHeaderEndpointBehavior();
        }
    }
}

