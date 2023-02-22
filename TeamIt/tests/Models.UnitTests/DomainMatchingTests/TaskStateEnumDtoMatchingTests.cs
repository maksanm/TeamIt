using Domain.Enums;
using Models.Enums;

namespace Models.IntegrationTests.DomainMatchingTests
{
    public class TaskStateEnumDtoMatchingTests
    {
        private readonly AssertionHelper _helper = new AssertionHelper();

        [Test]
        public void ShouldMatchDomainTaskStateEnum()
        {
            _helper.AssertEnumsEquality(typeof(TaskStateEnum), typeof(TaskStateEnumDto));
        }
    }
}