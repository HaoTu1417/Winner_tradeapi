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

    private bool FieldExists(string field)
    {
      return _db.KeyExists(stock_code) && _db.HashExists(stock_code, field);
    }

    public string stock_name
    {
      get => FieldExists(nameof(stock_name)) ? (string)_db.HashGet(stock_code, nameof(stock_name)) : string.Empty;
      set => _db.HashSetAsync(stock_code, nameof(stock_name), value);
    }

    public string exchange
    {
      get => FieldExists(nameof(exchange)) ? (string)_db.HashGet(stock_code, nameof(exchange)) : string.Empty;
      set => _db.HashSetAsync(stock_code, nameof(exchange), value);
    }

    public DateTime update_time
    {
      get
      {
        if (FieldExists(nameof(update_time)) && DateTime.TryParse((string)_db.HashGet(stock_code, nameof(update_time)), out var result))
          return result;
        return DateTime.MinValue;
      }
      set => _db.HashSetAsync(stock_code, nameof(update_time), value.ToString("yyyy-MM-ddTHH:mm:ss"));
    }

    public Decimal prev_day_c
    {
      get => FieldExists(nameof(prev_day_c)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(prev_day_c))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(prev_day_c), value.ToString());
    }

    public Decimal prev_day_v
    {
      get => FieldExists(nameof(prev_day_v)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(prev_day_v))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(prev_day_v), value.ToString());
    }

    public Decimal day_o
    {
      get => FieldExists(nameof(day_o)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(day_o))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(day_o), value.ToString());
    }

    public Decimal day_h
    {
      get => FieldExists(nameof(day_h)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(day_h))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(day_h), value.ToString());
    }

    public Decimal day_l
    {
      get => FieldExists(nameof(day_l)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(day_l))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(day_l), value.ToString());
    }

    public Decimal day_c
    {
      get => FieldExists(nameof(day_c)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(day_c))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(day_c), value.ToString());
    }

    public Decimal day_v
    {
      get => FieldExists(nameof(day_v)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(day_v))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(day_v), value.ToString());
    }

    public Decimal[] bids
    {
      get
      {
        if (FieldExists(nameof(bids)))
        {
          var raw = (string)_db.HashGet(stock_code, nameof(bids));
          return JsonConvert.DeserializeObject<Decimal[]>(raw) ?? Array.Empty<Decimal>();
        }
        return Array.Empty<Decimal>();
      }
      set => _db.HashSetAsync(stock_code, nameof(bids), JsonConvert.SerializeObject(value));
    }

    public Decimal[] asks
    {
      get
      {
        if (FieldExists(nameof(asks)))
        {
          var raw = (string)_db.HashGet(stock_code, nameof(asks));
          return JsonConvert.DeserializeObject<Decimal[]>(raw) ?? Array.Empty<Decimal>();
        }
        return Array.Empty<Decimal>();
      }
      set => _db.HashSetAsync(stock_code, nameof(asks), JsonConvert.SerializeObject(value));
    }

    public int[] bid_sizes
    {
      get
      {
        if (FieldExists(nameof(bid_sizes)))
        {
          var raw = (string)_db.HashGet(stock_code, nameof(bid_sizes));
          return JsonConvert.DeserializeObject<int[]>(raw) ?? Array.Empty<int>();
        }
        return Array.Empty<int>();
      }
      set => _db.HashSetAsync(stock_code, nameof(bid_sizes), JsonConvert.SerializeObject(value));
    }

    public int[] ask_sizes
    {
      get
      {
        if (FieldExists(nameof(ask_sizes)))
        {
          var raw = (string)_db.HashGet(stock_code, nameof(ask_sizes));
          return JsonConvert.DeserializeObject<int[]>(raw) ?? Array.Empty<int>();
        }
        return Array.Empty<int>();
      }
      set => _db.HashSetAsync(stock_code, nameof(ask_sizes), JsonConvert.SerializeObject(value));
    }

    public Decimal price
    {
      get => FieldExists(nameof(price)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(price))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(price), value.ToString());
    }

    public Decimal ceiling
    {
      get => FieldExists(nameof(ceiling)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(ceiling))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(ceiling), value.ToString());
    }

    public Decimal floor
    {
      get => FieldExists(nameof(floor)) ? Convert.ToDecimal((string)_db.HashGet(stock_code, nameof(floor))) : 0;
      set => _db.HashSetAsync(stock_code, nameof(floor), value.ToString());
    }

    public void Update(KeyValuePair<string, string>[] pairs)
    {
      HashEntry[] hashFields = new HashEntry[pairs.Length];
      for (int i = 0; i < pairs.Length; i++)
      {
        hashFields[i] = new HashEntry(pairs[i].Key, pairs[i].Value);
      }
      _db.HashSetAsync(stock_code, hashFields);
    }
  }
}
