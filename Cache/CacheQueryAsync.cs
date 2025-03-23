// Decompiled with JetBrains decompiler
// Type: tradeapi.Cache.CacheQueryAsync
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Cache
{
  public static class CacheQueryAsync
  {
    public static IDatabase? _db;

    public static void SelectDB(string market)
    {
      if (market.Text().ToUpper() == "VN")
        CacheQueryAsync._db = RedisEntity.SelectDb(5);
      else
        CacheQueryAsync._db = RedisEntity.SelectDb(2);
    }

    public static void SelectDB(int id) => CacheQueryAsync._db = RedisEntity.SelectDb(id);

    public static IBatch CreateBatch() => CacheQueryAsync._db?.CreateBatch();

    public static async Task<RedisResult> RunScriptAsync(LuaScript script, object? param)
    {
      return await CacheQueryAsync._db.ScriptEvaluateAsync(script, param);
    }

    public static IEnumerable<string> ConvertStrings<T>(IEnumerable<T> list) where T : struct
    {
      return list != null ? list.Select<T, string>((Func<T, string>) (x => x.ToString())) : throw new ArgumentNullException(nameof (list));
    }

    public static byte[] Serialize(object obj)
    {
      if (obj == null)
        return (byte[]) null;
      using (MemoryStream utf8Json = new MemoryStream())
      {
        JsonSerializer.Serialize<object>((Stream) utf8Json, obj);
        return utf8Json.ToArray();
      }
    }

    public static T Deserialize<T>(byte[] data)
    {
      if (data == null)
        return default (T);
      using (MemoryStream utf8Json = new MemoryStream(data))
        return (T) JsonSerializer.Deserialize((Stream) utf8Json, typeof (T));
    }

    public static async Task<bool> StringSetAsync(
      string redisKey,
      string redisValue,
      TimeSpan? expiry = null)
    {
      return await CacheQueryAsync._db.StringSetAsync((RedisKey) redisKey, (RedisValue) redisValue, expiry);
    }

    public static async Task StringBatchSetAsync(
      Dictionary<string, string> redisKeyValue,
      TimeSpan? expiry = null)
    {
      IBatch batch = CacheQueryAsync._db.CreateBatch();
      List<Task> taskList = new List<Task>();
      foreach (KeyValuePair<string, string> keyValuePair in redisKeyValue)
        taskList.Add((Task) batch.StringSetAsync((RedisKey) keyValuePair.Key, (RedisValue) keyValuePair.Value));
      batch.Execute();
      await Task.WhenAll((IEnumerable<Task>) taskList);
    }

    public static async Task<bool> StringSetAsync(
      IEnumerable<KeyValuePair<string, string>> keyValuePairs)
    {
      IEnumerable<KeyValuePair<RedisKey, RedisValue>> source = keyValuePairs.Select<KeyValuePair<string, string>, KeyValuePair<RedisKey, RedisValue>>((Func<KeyValuePair<string, string>, KeyValuePair<RedisKey, RedisValue>>) (x => new KeyValuePair<RedisKey, RedisValue>((RedisKey) x.Key, (RedisValue) x.Value)));
      return await CacheQueryAsync._db.StringSetAsync(source.ToArray<KeyValuePair<RedisKey, RedisValue>>());
    }

    public static async Task<string> StringGetAsync(string redisKey, TimeSpan? expiry = null)
    {
      return (string) await CacheQueryAsync._db.StringGetAsync((RedisKey) redisKey);
    }

    public static async Task<bool> StringSetAsync<T>(
      string redisKey,
      T redisValue,
      TimeSpan? expiry = null)
    {
      byte[] numArray = CacheQueryAsync.Serialize((object) (T) redisValue);
      return await CacheQueryAsync._db.StringSetAsync((RedisKey) redisKey, (RedisValue) numArray, expiry);
    }

    public static async Task<T> StringGetAsync<T>(string redisKey, TimeSpan? expiry = null)
    {
      return CacheQueryAsync.Deserialize<T>((byte[]) await CacheQueryAsync._db.StringGetAsync((RedisKey) redisKey));
    }

    public static T StringGet<T>(string redisKey, TimeSpan? expiry = null)
    {
      return CacheQueryAsync.Deserialize<T>((byte[]) CacheQueryAsync._db.StringGet((RedisKey) redisKey));
    }

    public static async Task<bool> HashExistsAsync(string redisKey, string hashField)
    {
      return await CacheQueryAsync._db.HashExistsAsync((RedisKey) redisKey, (RedisValue) hashField);
    }

    public static async Task<bool> HashDeleteAsync(string redisKey, string hashField)
    {
      return await CacheQueryAsync._db.HashDeleteAsync((RedisKey) redisKey, (RedisValue) hashField);
    }

    public static async Task<long> HashDeleteAsync(
      string redisKey,
      IEnumerable<RedisValue> hashField)
    {
      return await CacheQueryAsync._db.HashDeleteAsync((RedisKey) redisKey, hashField.ToArray<RedisValue>());
    }

    public static async Task<bool> HashSetAsync(string redisKey, string hashField, string value)
    {
      return await CacheQueryAsync._db.HashSetAsync((RedisKey) redisKey, (RedisValue) hashField, (RedisValue) value);
    }

    public static async Task HashSetAsync(string redisKey, IEnumerable<HashEntry> hashFields)
    {
      await CacheQueryAsync._db.HashSetAsync((RedisKey) redisKey, hashFields.ToArray<HashEntry>());
    }

    public static async Task<RedisValue> HashGetAsync(string redisKey, string hashField)
    {
      return await CacheQueryAsync._db.HashGetAsync((RedisKey) redisKey, (RedisValue) hashField);
    }

    public static async Task<IEnumerable<RedisValue>> HashGetAsync(
      string redisKey,
      RedisValue[] hashField,
      string value)
    {
      return (IEnumerable<RedisValue>) await CacheQueryAsync._db.HashGetAsync((RedisKey) redisKey, hashField);
    }

    public static async Task<IEnumerable<RedisValue>> HashKeysAsync(string redisKey)
    {
      return (IEnumerable<RedisValue>) await CacheQueryAsync._db.HashKeysAsync((RedisKey) redisKey);
    }

    public static async Task<IEnumerable<RedisValue>> HashValuesAsync(string redisKey)
    {
      return (IEnumerable<RedisValue>) await CacheQueryAsync._db.HashValuesAsync((RedisKey) redisKey);
    }

    public static async Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value)
    {
      string json = Tool.ToJson((object) (T) value);
      return await CacheQueryAsync._db.HashSetAsync((RedisKey) redisKey, (RedisValue) hashField, (RedisValue) json);
    }

    public static async Task<T> HashGetAsync<T>(string redisKey, string hashField)
    {
      return Tool.FromJson<T>((string) await CacheQueryAsync._db.HashGetAsync((RedisKey) redisKey, (RedisValue) hashField));
    }

    public static async Task<string> ListLeftPopAsync(string redisKey)
    {
      return (string) await CacheQueryAsync._db.ListLeftPopAsync((RedisKey) redisKey);
    }

    public static async Task<string> ListRightPopAsync(string redisKey)
    {
      return (string) await CacheQueryAsync._db.ListRightPopAsync((RedisKey) redisKey);
    }

    public static async Task<long> ListRemoveAsync(string redisKey, string redisValue)
    {
      return await CacheQueryAsync._db.ListRemoveAsync((RedisKey) redisKey, (RedisValue) redisValue);
    }

    public static async Task<long> ListRightPushAsync(string redisKey, string redisValue)
    {
      return await CacheQueryAsync._db.ListRightPushAsync((RedisKey) redisKey, (RedisValue) redisValue);
    }

    public static async Task<long> ListLeftPushAsync(string redisKey, string redisValue)
    {
      return await CacheQueryAsync._db.ListLeftPushAsync((RedisKey) redisKey, (RedisValue) redisValue);
    }

    public static async Task<long> ListLengthAsync(string redisKey)
    {
      return await CacheQueryAsync._db.ListLengthAsync((RedisKey) redisKey);
    }

    public static async Task<IEnumerable<string>> ListRangeAsync(
      string redisKey,
      long start = 0,
      long stop = -1)
    {
      return ((IEnumerable<RedisValue>) await CacheQueryAsync._db.ListRangeAsync((RedisKey) redisKey, start, stop)).Select<RedisValue, string>((Func<RedisValue, string>) (x => x.ToString()));
    }

    public static async Task<T> ListLeftPopAsync<T>(string redisKey)
    {
      return CacheQueryAsync.Deserialize<T>((byte[]) await CacheQueryAsync._db.ListLeftPopAsync((RedisKey) redisKey));
    }

    public static async Task<T> ListRightPopAsync<T>(string redisKey)
    {
      return CacheQueryAsync.Deserialize<T>((byte[]) await CacheQueryAsync._db.ListRightPopAsync((RedisKey) redisKey));
    }

    public static async Task<long> ListRightPushAsync<T>(string redisKey, T redisValue)
    {
      return await CacheQueryAsync._db.ListRightPushAsync((RedisKey) redisKey, (RedisValue) CacheQueryAsync.Serialize((object) (T) redisValue));
    }

    public static async Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue)
    {
      return await CacheQueryAsync._db.ListLeftPushAsync((RedisKey) redisKey, (RedisValue) CacheQueryAsync.Serialize((object) (T) redisValue));
    }

    public static async Task<bool> SortedSetAddAsync(string redisKey, string member, double score)
    {
      return await CacheQueryAsync._db.SortedSetAddAsync((RedisKey) redisKey, (RedisValue) member, score);
    }

    public static async Task<IEnumerable<string>> SortedSetRangeByRankAsync(string redisKey)
    {
      return CacheQueryAsync.ConvertStrings<RedisValue>((IEnumerable<RedisValue>) await CacheQueryAsync._db.SortedSetRangeByRankAsync((RedisKey) redisKey));
    }

    public static async Task<long> SortedSetLengthAsync(string redisKey)
    {
      return await CacheQueryAsync._db.SortedSetLengthAsync((RedisKey) redisKey);
    }

    public static async Task<bool> SortedSetRemoveAsync(string redisKey, string memebr)
    {
      return await CacheQueryAsync._db.SortedSetRemoveAsync((RedisKey) redisKey, (RedisValue) memebr);
    }

    public static async Task<bool> SortedSetAddAsync<T>(string redisKey, T member, double score)
    {
      byte[] member1 = CacheQueryAsync.Serialize((object) (T) member);
      return await CacheQueryAsync._db.SortedSetAddAsync((RedisKey) redisKey, (RedisValue) member1, score);
    }

    public static Task<double> SortedSetIncrementAsync(
      string redisKey,
      string member,
      double value = 1.0)
    {
      return CacheQueryAsync._db.SortedSetIncrementAsync((RedisKey) redisKey, (RedisValue) member, value);
    }

    public static async Task<bool> KeyDeleteAsync(string redisKey)
    {
      return await CacheQueryAsync._db.KeyDeleteAsync((RedisKey) redisKey);
    }

    public static async Task<long> KeyDeleteAsync(IEnumerable<string> redisKeys)
    {
      IEnumerable<RedisKey> source = redisKeys.Select<string, RedisKey>((Func<string, RedisKey>) (x => (RedisKey) x));
      return await CacheQueryAsync._db.KeyDeleteAsync(source.ToArray<RedisKey>());
    }

    public static async Task<bool> KeyExistsAsync(string redisKey)
    {
      return await CacheQueryAsync._db.KeyExistsAsync((RedisKey) redisKey);
    }

    public static async Task<bool> KeyRenameAsync(string redisKey, string redisNewKey)
    {
      return await CacheQueryAsync._db.KeyRenameAsync((RedisKey) redisKey, (RedisKey) redisNewKey);
    }

    public static async Task<bool> KeyExpireAsync(string redisKey, TimeSpan? expiry)
    {
      return await CacheQueryAsync._db.KeyExpireAsync((RedisKey) redisKey, expiry);
    }
  }
}
