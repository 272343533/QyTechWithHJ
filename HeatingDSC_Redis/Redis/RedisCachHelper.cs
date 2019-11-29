using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Redis;
using ServiceStack.Redis;
using System.Configuration;

using log4net;


namespace QyTech.Redis
{
    public class qyRedisCacheHelper
    {
        public static ILog log = log4net.LogManager.GetLogger("qyRedis");

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string redisConnString = ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString;


        private static readonly PooledRedisClientManager pool = null;
        private static readonly string[] redisHosts = null;
        public static int RedisMaxReadPool = 3;
        public static int RedisMaxWritePool = 1;

        static qyRedisCacheHelper()
        {
            var redisHostStr = redisConnString;// "127.0.0.1:6379";

            if (!string.IsNullOrEmpty(redisHostStr))
            {
                redisHosts = redisHostStr.Split(',');

                if (redisHosts.Length > 0)
                {
                    pool = new PooledRedisClientManager(redisHosts, redisHosts,
                        new RedisClientManagerConfig()
                        {
                            MaxWritePoolSize = RedisMaxWritePool,
                            MaxReadPoolSize = RedisMaxReadPool,
                            AutoStart = true
                        });
                }
            }
        }

        #region Add
        public static void Add<T>(string key, T value, DateTime expiry)
        {
            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {

                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, expiry - DateTime.Now);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                log.Error(key + ":" + ex.Message);
            }

        }


        public static void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, slidingExpiration);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                log.Error(key + ":" + ex.Message);
            }
        }



        public static void AddHash(string hashid, string key, string value)
        {
            if (value == null)
            {
                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.SetEntryInHashIfNotExists(hashid,key,value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                log.Error(key+":"+ex.Message);
            }

        }

        public static void Add2Hash(string hashid,string key,string value)
        {
            if (value == null)
            {
                return;
            }
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.SetEntryInHash(hashid,key, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                log.Error(key + ":" + ex.Message);
            }

        }
        
        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T obj = default(T);

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            obj = r.Get<T>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                 string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
                log.Error(key + ":" + ex.Message);
            }


            return obj;
        }

        public static string GetHash(string hashid,string key)
        {
            string obj = "";
            if (string.IsNullOrEmpty(key))
            {
                return obj;
            }

            
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            //long i = r.GetHashCount(key);
                            //List<String> obj1 = r.GetHashKeys(key);//.Get<T>(key);
                            //List<string> obj2 = r.GetHashValues(key);
                            obj = r.GetValueFromHash(hashid,key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
                log.Error(hashid+"_"+key + ":" + ex.Message);
            }


            return obj;
        }

        public static List<string> GetList(string listid)
        {
            List<string> obj =new List<string>();
            if (string.IsNullOrEmpty(listid))
            {
                return obj;
            }
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            obj = r.GetAllItemsFromList(listid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", listid);
                log.Error(listid + ":" + ex.Message);
            }


            return obj;
        }


        #endregion

        public static void Remove(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
                log.Error(key + ":" + ex.Message);
            }

        }

        public static void RemoveHashItem(string hashid,string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.RemoveEntryFromHash(hashid,key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
                log.Error(hashid + "_" + key + ":" + ex.Message);
            }

        }
        public static bool Exists(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.ContainsKey(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
                log.Error(key + ":" + ex.Message);
            }

            return false;
        }

        public static bool Exists(string hashid,string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.HashContainsEntry(hashid, key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
                log.Error(hashid + "_" + key + ":" + ex.Message);
            }

            return false;
        }
    }
}
