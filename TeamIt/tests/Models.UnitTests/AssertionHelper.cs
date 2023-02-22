using Domain.Enums;
using Models.Enums;

namespace Models.IntegrationTests
{
    public class AssertionHelper
    {
        public void AssertEnumsEquality(Type domainEnumType, Type dtoEnumType)
        {
            var domainNames = Enum.GetNames(domainEnumType);
            var dtoNames = Enum.GetNames(dtoEnumType);

            Assert.That(dtoNames.Length, Is.EqualTo(domainNames.Length),
                $"{typeof(PermissionEnumDto).Name} must have the same number of values as {typeof(PermissionEnum).Name}");
            for (int i = 0; i < domainNames.Length; i++)
                Assert.That(dtoNames[i], Is.EqualTo(domainNames[i]),
                    $"{typeof(PermissionEnumDto).Name} value name should be same as corresponding {typeof(PermissionEnum).Name} value name: " +
                    $"{dtoNames[i]} is not equal to {domainNames[i]}");
        }
    }
}