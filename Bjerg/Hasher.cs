using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Bjerg
{
    /// <summary>
    /// A wrapper around an <see cref="int"/> that exposes fluid methods for non-cryptographically hashing multiple objects. All methods use the FNV-1a function.
    /// </summary>
    public readonly struct Hasher : IEquatable<Hasher>
    {
        private readonly int _code;

        private Hasher(int code)
        {
            this._code = code;
        }

        public static int ToInt32(Hasher hasher)
        {
            return hasher._code;
        }

        public static implicit operator int(Hasher hasher) => ToInt32(hasher);

        public override string ToString()
        {
            return _code.ToString("X8", System.Globalization.CultureInfo.InvariantCulture);
        }

        #region Equality

        public override int GetHashCode()
        {
            return _code;
        }

        public bool Equals(Hasher other)
        {
            return this._code == other._code;
        }

        public override bool Equals(object? obj)
        {
            return obj is Hasher other && this.Equals(other);
        }

        public static bool operator ==(Hasher lhs, Hasher rhs) => lhs.Equals(rhs);

        public static bool operator !=(Hasher lhs, Hasher rhs) => !lhs.Equals(rhs);

        #endregion

        #region ✨ Magic ✨

        private const int _fnvBasis = unchecked((int)0x811c9dc5);
        private const int _fnvPrime = 0x01000193; // 2^24 + 2^8 + (2^7 + 2^4 + 2^1 + 2^0)

        private const byte _falseCode        = 0x03;
        private const byte _trueCode         = 0x65;
        private const byte _nullObjectCode   = 0x9d;
        private const byte _nullSequenceCode = 0xfb;

        #endregion

        /// <summary>
        /// A <see cref="Hasher"/> wrapping an offset basis.
        /// </summary>
        public static Hasher Start => new Hasher(_fnvBasis);

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Hash8(int c, byte v)
        {
            // XOR first, multiply second per FNV-1a
            return unchecked((c ^ v) * _fnvPrime);
        }

        private static int Hash32(int c, int v)
        {
            const int size = sizeof(int);

            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, v);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return c;
        }

        private static int HashNullable<T>(int c, T v)
        {
            return v is null ? Hash8(c, _nullObjectCode) : Hash32(c, v.GetHashCode());
        }

        private static int HashNullable<T>(int c, T? v, IEqualityComparer<T> comparer) where T : struct
        {
            return v.HasValue ? Hash32(c, comparer.GetHashCode(v.Value)) : Hash8(c, _nullObjectCode);
        }

        private static int HashNullable<T>(int c, T? v, IEqualityComparer<T> comparer) where T : class
        {
            return v is null ? Hash8(c, _nullObjectCode) : Hash32(c, comparer.GetHashCode(v));
        }

        #endregion

        #region Primitive Hashing

        /// <summary>
        /// Hashes a <see cref="byte"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(byte item)
        {
            int c = this._code;
            c = Hash8(c, item);
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="sbyte"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(sbyte item)
        {
            int c = this._code;
            c = Hash8(c, (byte)item);
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="bool"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(bool item)
        {
            int c = this._code;
            c = Hash8(c, item ? _trueCode : _falseCode);
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="int"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(int item)
        {
            int c = this._code;
            c = Hash32(c, item);
            return new Hasher(c);
        }

        // The other multi-byte primitive types share a common template based on BitConverter.TryWriteBytes
        // Let me eat my cake, C# (๑◕︵◕๑)

        /// <summary>
        /// Hashes a <see cref="char"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(char item)
        {
            const int size = sizeof(char);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="short"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(short item)
        {
            const int size = sizeof(short);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="long"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(long item)
        {
            const int size = sizeof(long);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="ushort"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(ushort item)
        {
            const int size = sizeof(ushort);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="uint"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(uint item)
        {
            const int size = sizeof(uint);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="ulong"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(ulong item)
        {
            const int size = sizeof(ulong);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="float"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(float item)
        {
            const int size = sizeof(float);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        /// <summary>
        /// Hashes a <see cref="double"/> primitive.
        /// </summary>
        /// <param name="item">The value to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash(double item)
        {
            const int size = sizeof(double);

            int c = this._code;
            Span<byte> bytes = stackalloc byte[size];
            _ = BitConverter.TryWriteBytes(bytes, item);
            for (int i = 0; i < size; i++)
            {
                c = Hash8(c, bytes[i]);
            }
            return new Hasher(c);
        }

        #endregion

        #region Generic Hashing

        /// <summary>
        /// Hashes a generic struct object using its override of <see cref="object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="item">The object to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher Hash<T>(T item) where T : struct
        {
            int v = item.GetHashCode();
            int code = Hash32(this._code, v);
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes a generic struct object using <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="item">The object to hash.</param>
        /// <param name="comparer">The comparer whose hashing function to use.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        /// <exception cref="ArgumentNullException">The equality comparer is null.</exception>
        public Hasher Hash<T>(T item, [DisallowNull] IEqualityComparer<T> comparer) where T : struct
        {
            if (comparer is null) { throw new ArgumentNullException(nameof(comparer)); }

            int v = comparer.GetHashCode(item);
            int code = Hash32(this._code, v);
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes a nullable generic struct object using its override of <see cref="object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="item">The object to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher HashNullable<T>(T? item) where T : struct
        {
            int code = HashNullable(this._code, item);
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes a generic class object using its override of <see cref="object.GetHashCode"/>.
        /// </summary>
        /// <typeparam name="T">Any reference type.</typeparam>
        /// <param name="item">The object to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher HashNullable<T>(T? item) where T : class
        {
            int code = HashNullable(this._code, item);
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes a nullable generic struct object using <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="item">The object to hash.</param>
        /// <param name="comparer">The comparer whose hashing function to use.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        /// <exception cref="ArgumentNullException">The equality comparer is null.</exception>
        public Hasher HashNullable<T>(T? item, [DisallowNull] IEqualityComparer<T> comparer) where T : struct
        {
            if (comparer is null) { throw new ArgumentNullException(nameof(comparer)); }

            int code = HashNullable(this._code, item, comparer);
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes a class object using <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// </summary>
        /// <typeparam name="T">Any reference type.</typeparam>
        /// <param name="item">The object to hash.</param>
        /// <param name="comparer">The comparer whose hashing function to use.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        /// <exception cref="ArgumentNullException">The equality comparer is null.</exception>
        public Hasher HashNullable<T>(T? item, [DisallowNull] IEqualityComparer<T> comparer) where T : class
        {
            if (comparer is null) { throw new ArgumentNullException(nameof(comparer)); }

            int code = HashNullable(this._code, item, comparer);
            return new Hasher(code);
        }

        #endregion

        #region Generic Sequence Hashing

        /// <summary>
        /// Hashes each struct object in a sequence using its override of <see cref="object.GetHashCode"/>. A <see cref="null"/> sequence and an empty sequence always return different hashes.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="items">The sequence of objects to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher HashSequence<T>([AllowNull] IEnumerable<T> items) where T : struct
        {
            int code = this._code;
            if (items is null)
            {
                return new Hasher(Hash8(code, _nullSequenceCode));
            }
            foreach (T item in items)
            {
                code = Hash32(code, item.GetHashCode());
            }
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes each struct object in a sequence using <see cref="IEqualityComparer{T}.GetHashCode(T)"/>. A <see cref="null"/> sequence and an empty sequence always return different hashes.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="items">The sequence of objects to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        /// <exception cref="ArgumentNullException">The equality comparer is null.</exception>
        public Hasher HashSequence<T>([AllowNull] IEnumerable<T> items, [DisallowNull] IEqualityComparer<T> comparer) where T : struct
        {
            if (comparer is null) { throw new ArgumentNullException(nameof(comparer)); }

            int code = this._code;
            if (items is null)
            {
                return new Hasher(Hash8(code, _nullSequenceCode));
            }
            foreach (T item in items)
            {
                code = Hash32(code, comparer.GetHashCode(item));
            }
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes each nullable struct object in a sequence using its override of <see cref="object.GetHashCode"/>. A <see cref="null"/> sequence, an empty sequence, and a sequence with a single item that's <see cref="null"/> always return different hashes.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="items">The sequence of objects to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher HashNullableSequence<T>([AllowNull] IEnumerable<T?> items) where T : struct
        {
            int code = this._code;
            if (items is null)
            {
                return new Hasher(Hash8(code, _nullSequenceCode));
            }
            foreach (T? item in items)
            {
                code = HashNullable(code, item);
            }
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes each class object in a sequence using its override of <see cref="object.GetHashCode"/>. A <see cref="null"/> sequence, an empty sequence, and a sequence with a single item that's <see cref="null"/> always return different hashes.
        /// </summary>
        /// <typeparam name="T">Any reference type.</typeparam>
        /// <param name="items">The sequence of objects to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        public Hasher HashNullableSequence<T>([AllowNull] IEnumerable<T?> items) where T : class
        {
            int code = this._code;
            if (items is null)
            {
                return new Hasher(Hash8(code, _nullSequenceCode));
            }
            foreach (T? item in items)
            {
                code = HashNullable(code, item);
            }
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes each nullable struct object in a sequence using <see cref="IEqualityComparer{T}.GetHashCode(T)"/>. A <see cref="null"/> sequence, an empty sequence, and a sequence with a single item that's <see cref="null"/> always return different hashes.
        /// </summary>
        /// <typeparam name="T">Any value type.</typeparam>
        /// <param name="items">The sequence of objects to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        /// <exception cref="ArgumentNullException">The equality comparer is null.</exception>
        public Hasher HashNullableSequence<T>([AllowNull] IEnumerable<T?> items, [DisallowNull] IEqualityComparer<T> comparer) where T : struct
        {
            if (comparer is null) { throw new ArgumentNullException(nameof(comparer)); }

            int code = this._code;
            if (items is null)
            {
                return new Hasher(Hash8(code, _nullSequenceCode));
            }
            foreach (T? item in items)
            {
                code = HashNullable(code, item, comparer);
            }
            return new Hasher(code);
        }

        /// <summary>
        /// Hashes each class object in a sequence using <see cref="IEqualityComparer{T}.GetHashCode(T)"/>. A <see cref="null"/> sequence, an empty sequence, and a sequence with a single item that's <see cref="null"/> always return different hashes.
        /// </summary>
        /// <typeparam name="T">Any reference type.</typeparam>
        /// <param name="items">The sequence of objects to hash.</param>
        /// <returns>A new <see cref="Hasher"/>.</returns>
        /// <exception cref="ArgumentNullException">The equality comparer is null.</exception>
        public Hasher HashNullableSequence<T>([AllowNull] IEnumerable<T?> items, [DisallowNull] IEqualityComparer<T> comparer) where T : class
        {
            if (comparer is null) { throw new ArgumentNullException(nameof(comparer)); }

            int code = this._code;
            if (items is null)
            {
                return new Hasher(Hash8(code, _nullSequenceCode));
            }
            foreach (T? item in items)
            {
                code = HashNullable(code, item, comparer);
            }
            return new Hasher(code);
        }

        #endregion
    }
}
