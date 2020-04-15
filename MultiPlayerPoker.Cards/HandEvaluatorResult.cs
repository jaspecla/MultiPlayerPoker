using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Cards
{

  public class HandEvaluatorResult : IComparable<HandEvaluatorResult>
  {

    public HandType Hand { get; set; }
    public int Determinant { get; set;  }
    public int SubDeterminant { get; set; }
    public int Kicker { get; set; }
    public int SecondKicker { get; set; }
    public int ThirdKicker { get; set; }
    public int FourthKicker { get; set; }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (ReferenceEquals(obj, null))
      {
        return false;
      }

      var other = obj as HandEvaluatorResult;
      if (other == null)
      {
        return false;
      }

      return (this.CompareTo(other) == 0);
    }

    public int CompareTo(HandEvaluatorResult other)
    {
      if (other == null)
      {
        return 1;
      }

      if (this.Hand > other.Hand)
      {
        return 1;
      }
      else if (this.Hand < other.Hand)
      {
        return -1;
      }

      else // the hands are equal
      {
        switch (this.Hand)
        {
          // Royal Flushes are all equal value.
          // Dang it would be amazing if we hit this ccase.
          case HandType.RoyalFlush:
            return 0;
          case HandType.StraightFlush:
            return this.Determinant.CompareTo(other.Determinant);
          case HandType.FourOfAKind:
            if (this.Determinant != other.Determinant)
            {
              return this.Determinant.CompareTo(other.Determinant);
            }
            else
            {
              return this.Kicker.CompareTo(other.Kicker);
            }
          case HandType.FullHouse:
            if (this.Determinant != other.Determinant)
            {
              return this.Determinant.CompareTo(other.Determinant);
            }
            else
            {
              return this.SubDeterminant.CompareTo(other.SubDeterminant);
            }
          case HandType.Flush:
            return this.Determinant.CompareTo(other.Determinant);
          case HandType.Straight:
            return this.Determinant.CompareTo(other.Determinant);
          case HandType.ThreeOfAKind:
            if (this.Determinant != other.Determinant)
            {
              return this.Determinant.CompareTo(other.Determinant);
            }
            else if (this.Kicker != other.Kicker)
            {
              return this.Kicker.CompareTo(other.Kicker);
            }
            else if (this.SecondKicker != other.SecondKicker)
            {
              return this.SecondKicker.CompareTo(other.SecondKicker);
            }
            else
            {
              return 0;
            }
          case HandType.TwoPair:
            if (this.Determinant != other.Determinant)
            {
              return this.Determinant.CompareTo(other.Determinant);
            }
            else if (this.SubDeterminant != other.SubDeterminant)
            {
              return this.SubDeterminant.CompareTo(other.SubDeterminant);
            }
            else if (this.Kicker != other.Kicker)
            {
              return this.Kicker.CompareTo(other.Kicker);
            }
            else
            {
              return 0;
            }
          case HandType.Pair:
            if (this.Determinant != other.Determinant)
            {
              return this.Determinant.CompareTo(other.Determinant);
            }
            else if (this.Kicker != other.Kicker)
            {
              return this.Kicker.CompareTo(other.Kicker);
            }
            else if (this.SecondKicker != other.SecondKicker)
            {
              return this.SecondKicker.CompareTo(other.SecondKicker);
            }
            else if (this.ThirdKicker != other.ThirdKicker)
            {
              return this.ThirdKicker.CompareTo(other.ThirdKicker);
            }
            else
            {
              return 0;
            }
          case HandType.High:
            if (this.Determinant != other.Determinant)
            {
              return this.Determinant.CompareTo(other.Determinant);
            } 
            else if (this.Kicker != other.Kicker)
            {
              return this.Kicker.CompareTo(other.Kicker);
            }
            else if (this.SecondKicker != other.SecondKicker)
            {
              return this.SecondKicker.CompareTo(other.SecondKicker);
            }
            else if (this.ThirdKicker != other.ThirdKicker)
            {
              return this.ThirdKicker.CompareTo(other.ThirdKicker);
            }
            else if (this.FourthKicker != other.FourthKicker)
            {
              return this.FourthKicker.CompareTo(other.FourthKicker);
            }
            else
            {
              return 0;
            }
          case HandType.INVALID_HAND:
            return 0;
          default:
            return 0;
        }
      }
    }

    // Automatically generated
    public override int GetHashCode()
    {
      int hashCode = -1902641761;
      hashCode = hashCode * -1521134295 + Hand.GetHashCode();
      hashCode = hashCode * -1521134295 + Determinant.GetHashCode();
      hashCode = hashCode * -1521134295 + SubDeterminant.GetHashCode();
      hashCode = hashCode * -1521134295 + Kicker.GetHashCode();
      hashCode = hashCode * -1521134295 + SecondKicker.GetHashCode();
      hashCode = hashCode * -1521134295 + ThirdKicker.GetHashCode();
      hashCode = hashCode * -1521134295 + FourthKicker.GetHashCode();
      return hashCode;
    }

    public static bool operator ==(HandEvaluatorResult left, HandEvaluatorResult right)
    {
      if (ReferenceEquals(left, null))
      {
        return ReferenceEquals(right, null);
      }

      return left.Equals(right);
    }

    public static bool operator !=(HandEvaluatorResult left, HandEvaluatorResult right)
    {
      return !(left == right);
    }

    public static bool operator <(HandEvaluatorResult left, HandEvaluatorResult right)
    {
      return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
    }

    public static bool operator <=(HandEvaluatorResult left, HandEvaluatorResult right)
    {
      return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
    }

    public static bool operator >(HandEvaluatorResult left, HandEvaluatorResult right)
    {
      return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
    }

    public static bool operator >=(HandEvaluatorResult left, HandEvaluatorResult right)
    {
      return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
  }
}
