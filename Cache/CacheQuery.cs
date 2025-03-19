// Decompiled with JetBrains decompiler
// Type: tradeapi.Cache.CacheQuery
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: C:\Users\VN6\Documents\Projects\Winner Dotnet\service\tradeapi\tradeapi.dll

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Cache
{
  public class CacheQuery
  {
    private static IDatabase _db = RedisEntity.SelectDb(2);

    public static void SelectDB(int id) => CacheQuery._db = RedisEntity.SelectDb(id);

    public static IBatch CreateBatch() => CacheQuery._db.CreateBatch();

    public static RedisResult RunScript(LuaScript script, object? param)
    {
      return CacheQuery._db.ScriptEvaluate(script, param);
    }

    private static IEnumerable<string> ConvertStrings<T>(IEnumerable<T> list) where T : struct
    {
      return list != null ? list.Select<T, string>((Func<T, string>) (x => x.ToString())) : throw new ArgumentNullException(nameof (list));
    }

    private static byte[] Serialize(object obj)
    {
      if (obj == null)
        return (byte[]) null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        JsonSerializer.Serialize<object>((Stream) memoryStream, obj, (JsonSerializerOptions) null);
        return memoryStream.ToArray();
      }
    }

    private static T Deserialize<T>(byte[] data)
    {
      if (data == null)
        return default (T);
      using (MemoryStream memoryStream = new MemoryStream(data))
        return (T) JsonSerializer.Deserialize((Stream) memoryStream, typeof (T), (JsonSerializerOptions) null);
    }

    public static bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null)
    {
      return CacheQuery._db.StringSet((RedisKey) redisKey, (RedisValue) redisValue, expiry);
    }

    public static bool StringSet(
      IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs)
    {
      keyValuePairs = keyValuePairs.Select<KeyValuePair<RedisKey, RedisValue>, KeyValuePair<RedisKey, RedisValue>>((Func<KeyValuePair<RedisKey, RedisValue>, KeyValuePair<RedisKey, RedisValue>>) (x => new KeyValuePair<RedisKey, RedisValue>(x.Key, x.Value)));
      return CacheQuery._db.StringSet(keyValuePairs.ToArray<KeyValuePair<RedisKey, RedisValue>>());
    }

    public static string StringGet(string redisKey, TimeSpan? expiry = null)
    {
      return (string) CacheQuery._db.StringGet((RedisKey) redisKey);
    }

    public static bool StringSet<T>(string redisKey, T redisValue, TimeSpan? expiry = null)
    {
      string json = Tool.ToJson((object) redisValue);
      return CacheQuery._db.StringSet((RedisKey) redisKey, (RedisValue) json, expiry);
    }

    public static T StringGet<T>(string redisKey, TimeSpan? expiry = null)
    {
      return Tool.FromJson<T>((string) CacheQuery._db.StringGet((RedisKey) redisKey));
    }

    public static bool HashExists(string redisKey, string hashField)
    {
      return CacheQuery._db.HashExists((RedisKey) redisKey, (RedisValue) hashField);
    }

    public static bool HashDelete(string redisKey, string hashField)
    {
      return CacheQuery._db.HashDelete((RedisKey) redisKey, (RedisValue) hashField);
    }

    public static long HashDelete(string redisKey, IEnumerable<RedisValue> hashField)
    {
      return CacheQuery._db.HashDelete((RedisKey) redisKey, hashField.ToArray<RedisValue>());
    }

    public static bool HashSet(string redisKey, string hashField, string value)
    {
      return CacheQuery._db.HashSet((RedisKey) redisKey, (RedisValue) hashField, (RedisValue) value);
    }

    public static void HashSet(string redisKey, IEnumerable<HashEntry> hashFields)
    {
      CacheQuery._db.HashSet((RedisKey) redisKey, hashFields.ToArray<HashEntry>());
    }

    public static RedisValue HashGet(string redisKey, string hashField)
    {
      return CacheQuery._db.HashGet((RedisKey) redisKey, (RedisValue) hashField);
    }

    public static RedisValue[] HashGet(string redisKey, RedisValue[] hashField, string value)
    {
      return CacheQuery._db.HashGet((RedisKey) redisKey, hashField);
    }

    public static IEnumerable<RedisValue> HashKeys(string redisKey)
    {
      return (IEnumerable<RedisValue>) CacheQuery._db.HashKeys((RedisKey) redisKey);
    }

    public static RedisValue[] HashValues(string redisKey)
    {
      return CacheQuery._db.HashValues((RedisKey) redisKey);
    }

    public static bool HashSet<T>(string redisKey, string hashField, T value)
    {
      string json = Tool.ToJson((object) value);
      return CacheQuery._db.HashSet((RedisKey) redisKey, (RedisValue) hashField, (RedisValue) json);
    }

    public static T HashGet<T>(string redisKey, string hashField)
    {
      return Tool.FromJson<T>((string) CacheQuery._db.HashGet((RedisKey) redisKey, (RedisValue) hashField));
    }

    public static string ListLeftPop(string redisKey)
    {
      return (string) CacheQuery._db.ListLeftPop((RedisKey) redisKey);
    }

    public static string ListRightPop(string redisKey)
    {
      return (string) CacheQuery._db.ListRightPop((RedisKey) redisKey);
    }

    public static long ListRemove(string redisKey, string redisValue)
    {
      return CacheQuery._db.ListRemove((RedisKey) redisKey, (RedisValue) redisValue);
    }

    public static long ListRightPush(string redisKey, string redisValue)
    {
      return CacheQuery._db.ListRightPush((RedisKey) redisKey, (RedisValue) redisValue);
    }

    public static long ListLeftPush(string redisKey, string redisValue)
    {
      return CacheQuery._db.ListLeftPush((RedisKey) redisKey, (RedisValue) redisValue);
    }

    public static long ListLength(string redisKey)
    {
      return CacheQuery._db.ListLength((RedisKey) redisKey);
    }

    public static IEnumerable<string> ListRange(string redisKey, long start = 0, long stop = -1)
    {
      return CacheQuery.ConvertStrings<RedisValue>((IEnumerable<RedisValue>) CacheQuery._db.ListRange((RedisKey) redisKey, start, stop));
    }

    public static T ListLeftPop<T>(string redisKey)
    {
      return CacheQuery.Deserialize<T>((byte[]) CacheQuery._db.ListLeftPop((RedisKey) redisKey));
    }

    public static T ListRightPop<T>(string redisKey)
    {
      return CacheQuery.Deserialize<T>((byte[]) CacheQuery._db.ListRightPop((RedisKey) redisKey));
    }

    public static long ListRightPush<T>(string redisKey, T redisValue)
    {
      return CacheQuery._db.ListRightPush((RedisKey) redisKey, (RedisValue) CacheQuery.Serialize((object) redisValue));
    }

    public static long ListLeftPush<T>(string redisKey, T redisValue)
    {
      return CacheQuery._db.ListLeftPush((RedisKey) redisKey, (RedisValue) CacheQuery.Serialize((object) redisValue));
    }

    public static bool SortedSetAdd(string redisKey, string member, double score)
    {
      return CacheQuery._db.SortedSetAdd((RedisKey) redisKey, (RedisValue) member, score);
    }

    public static IEnumerable<string> SortedSetRangeByRank(
      string redisKey,
      long start = 0,
      long stop = -1,
      tradeapi.Utility.SortType order = tradeapi.Utility.SortType.Ascending)
    {
      return ((IEnumerable<RedisValue>) CacheQuery._db.SortedSetRangeByRank((RedisKey) redisKey, start, stop, (Order) order)).Select<RedisValue, string>((Func<RedisValue, string>) (x => x.ToString()));
    }

    public static long SortedSetLength(string redisKey)
    {
      return CacheQuery._db.SortedSetLength((RedisKey) redisKey);
    }

    public static bool SortedSetLength(string redisKey, string memebr)
    {
      return CacheQuery._db.SortedSetRemove((RedisKey) redisKey, (RedisValue) memebr);
    }

    public static bool SortedSetAdd<T>(string redisKey, T member, double score)
    {
      byte[] member1 = CacheQuery.Serialize((object) member);
      return CacheQuery._db.SortedSetAdd((RedisKey) redisKey, (RedisValue) member1, score);
    }

    public static double SortedSetIncrement(string redisKey, string member, double value = 1.0)
    {
      return CacheQuery._db.SortedSetIncrement((RedisKey) redisKey, (RedisValue) member, value);
    }

    public static bool KeyDelete(string redisKey) => CacheQuery._db.KeyDelete((RedisKey) redisKey);

    public static long KeyDelete(IEnumerable<string> redisKeys)
    {
      IEnumerable<RedisKey> source = redisKeys.Select<string, RedisKey>((Func<string, RedisKey>) (x => (RedisKey) x));
      return CacheQuery._db.KeyDelete(source.ToArray<RedisKey>());
    }

    public static bool KeyExists(string redisKey) => CacheQuery._db.KeyExists((RedisKey) redisKey);

    public static bool KeyRename(string redisKey, string redisNewKey)
    {
      return CacheQuery._db.KeyRename((RedisKey) redisKey, (RedisKey) redisNewKey);
    }

    public static bool KeyExpire(string redisKey, TimeSpan? expiry)
    {
      return CacheQuery._db.KeyExpire((RedisKey) redisKey, expiry);
    }

    internal static void SelectDB(CacheEnum ce) => CacheQuery._db = RedisEntity.SelectDb((int) ce);
  }
}
