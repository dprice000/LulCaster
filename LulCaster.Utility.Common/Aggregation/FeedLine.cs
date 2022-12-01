using System;
using System.Security.Cryptography;
using System.Text;

namespace LulCaster.Utility.Common.Aggregation
{
  public class FeedLine
  {
    public readonly string attacker;
    public readonly string target;
    private readonly int _objectHash;

    public FeedLine(string attacker, string target)
    {
      this.attacker = attacker;
      this.target = target;
      _objectHash = GetHashAsInt(attacker + target);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is FeedLine)) return false;

      return obj.GetHashCode() == _objectHash;
    }

    public override int GetHashCode()
    {
      return _objectHash;
    }

    public static int GetHashAsInt(string inputString)
    {
      using (HashAlgorithm algorithm = SHA256.Create())
      {
        return BitConverter.ToInt32(algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString)));
      }
    }
  }
}