using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Reflection;
using System.Data.Objects.DataClasses;
using log4net;
using System.Data.Entity;
using System.Data;

namespace SunMvcExpress.Core.Models
{
    public static class ContextObject_Helper
    {
        public static ILog log = log4net.LogManager.GetLogger("ContextObject_Helper");

        /// <summary>
        /// 判断entity是否已经Attached
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsAttached<T>(this ObjectContext objectContext, T entity) where T : EntityObject
        {
            ObjectStateEntry entry = null;
            if (objectContext.ObjectStateManager.TryGetObjectStateEntry(entity.EntityKey, out entry))
            {
                if (entry.State != EntityState.Detached)
                {
                    return true;
                }
            }
            return false;
        }

        public static int UpdateEntity<T>(this ObjectContext objectContext, T entity, string entitySetName) where T : EntityObject
        {
            // Attach specify entity
            try
            {
                if (!IsAttached(objectContext, entity))
                    objectContext.AttachTo(entitySetName, entity);
            }
            catch(Exception ex){
                if (ex.Message != "无法附加此对象，因为它已经在对象上下文中。对象只有在处于未更改状态时才能重新附加。")
                    log.Error(ex.Message);
            }

            // Get object state by specify entity.
            try
            {
                System.Data.Objects.ObjectStateEntry state = objectContext.ObjectStateManager.GetObjectStateEntry(entity);

                // Get all members from entity.
                MemberInfo[] members = entity.GetType().GetMembers();

                // Get all properties except EntityKey.
                IEnumerable<MemberInfo> properties = members.Where(m => m.MemberType == MemberTypes.Property && m.Name != "EntityKey");

                foreach (MemberInfo mInfo in properties)
                {
                    //Find property that used System.Runtime.Serialization.DataMemberAttribute.
                    object[] attrs = mInfo.GetCustomAttributes(typeof(System.Runtime.Serialization.DataMemberAttribute), true);

                    if (attrs.Length > 0)
                    {
                        object[] emdAttrs = mInfo.GetCustomAttributes(typeof(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), true);
                        if (emdAttrs.Length > 0)
                        {
                            var edm = emdAttrs[0] as System.Data.Objects.DataClasses.EdmScalarPropertyAttribute;
                            if (!edm.EntityKeyProperty)
                                state.SetModifiedProperty(mInfo.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return 0;
        }
    }
}