// Decompiled with JetBrains decompiler
// Type: tradeapi.Models.StockQuote
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

#nullable enable
namespace tradeapi.Models
{
  public class StockQuote
  {
    private IDatabase _db;

    public string stock_code { get; }

    public StockQuote(IDatabase db, string stock_code)
    {
      this._db = db;
      this.stock_code = stock_code;
    }

    public string stock_name
    {
      get
      {
        return (string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (stock_name));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (stock_name), (RedisValue) value);
      }
    }

    public string exchange
    {
      get => (string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (exchange));
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (exchange), (RedisValue) value);
      }
    }

    public DateTime update_time
    {
      get
      {
        return Convert.ToDateTime((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (update_time)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (update_time), (RedisValue) value.ToString("yyyy-MM-ddTHH:mm:ss"));
      }
    }

    public Decimal prev_day_c
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (prev_day_c)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (prev_day_c), (RedisValue) value.ToString());
      }
    }

    public Decimal prev_day_v
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (prev_day_v)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (prev_day_v), (RedisValue) value.ToString());
      }
    }

    public Decimal day_o
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (day_o)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (day_o), (RedisValue) value.ToString());
      }
    }

    public Decimal day_h
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (day_h)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (day_h), (RedisValue) value.ToString());
      }
    }

    public Decimal day_l
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (day_l)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (day_l), (RedisValue) value.ToString());
      }
    }

    public Decimal day_c
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (day_c)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (day_c), (RedisValue) value.ToString());
      }
    }

    public Decimal day_v
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (day_v)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (day_v), (RedisValue) value.ToString());
      }
    }

    public Decimal[] bids
    {
      get
      {
        return JsonConvert.DeserializeObject<Decimal[]>((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (bids)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (bids), (RedisValue) JsonConvert.SerializeObject((object) value));
      }
    }

    public Decimal[] asks
    {
      get
      {
        return JsonConvert.DeserializeObject<Decimal[]>((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (asks)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (asks), (RedisValue) JsonConvert.SerializeObject((object) value));
      }
    }

    public int[] bid_sizes
    {
      get
      {
        return JsonConvert.DeserializeObject<int[]>((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (bid_sizes)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (bid_sizes), (RedisValue) JsonConvert.SerializeObject((object) value));
      }
    }

    public int[] ask_sizes
    {
      get
      {
        return JsonConvert.DeserializeObject<int[]>((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (ask_sizes)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (ask_sizes), (RedisValue) JsonConvert.SerializeObject((object) value));
      }
    }

    public Decimal price
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (price)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (price), (RedisValue) value.ToString());
      }
    }

    public Decimal ceiling
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (ceiling)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (ceiling), (RedisValue) value.ToString());
      }
    }

    public Decimal floor
    {
      get
      {
        return Convert.ToDecimal((string) this._db.HashGet((RedisKey) this.stock_code, (RedisValue) nameof (floor)));
      }
      set
      {
        this._db.HashSetAsync((RedisKey) this.stock_code, (RedisValue) nameof (floor), (RedisValue) value.ToString());
      }
    }

    public void Update(KeyValuePair<string, string>[] pairs)
    {
      HashEntry[] hashFields = new HashEntry[pairs.Length];
      for (int index = 0; index < hashFields.Length; ++index)
        hashFields[index] = new HashEntry((RedisValue) pairs[index].Key, (RedisValue) pairs[index].Value);
      this._db.HashSetAsync((RedisKey) this.stock_code, hashFields);
    }
  }
}
