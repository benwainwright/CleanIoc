namespace CleanIoc.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;

    public class AssemblyReference : EntityWithToken
    {
        public AssemblyReference(MetadataReader reader, AssemblyReferenceHandle handle)
            : base(reader.GetToken(handle))
        {
            var reference = reader.GetAssemblyReference(handle);
            Name = reader.GetString(reference.Name);
            Culture = reader.GetString(reference.Culture);
            PublicKeyOrToken = reader.GetBlobBytes(reference.PublicKeyOrToken).ToList();
            Version = reference.Version;
            Flags = reference.Flags;
        }

        public string Culture { get; }

        public string Name { get; }

        public List<byte> PublicKeyOrToken { get; }

        public Version Version { get; }

        public AssemblyFlags Flags { get; }

        public static List<AssemblyReference> LoadReferences(MetadataReader reader)
        {
            var query = from reference
                        in reader.AssemblyReferences
                        select new AssemblyReference(reader, reference);

            return query.ToList();
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }
    }
}
