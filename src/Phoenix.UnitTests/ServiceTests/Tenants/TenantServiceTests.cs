using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Phoenix.Application.Services.Tenants.Contracts;
using Phoenix.Application.Services.Tenants.Contracts.Dtos;
using Phoenix.Application.Services.Tenants.Exceptions;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.Domain.Entities.Tenants;
using Phoenix.TestTools.Infrastructure;
using Phoenix.TestTools.Tools.Buliders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Phoenix.UnitTests.ServiceTests.Tenants
{
    public class TenantServiceTests : UnitTestSut<TenantService>
    {
        [Fact]
        public async Task Add_add_tenant_properly()
        {
            var dto =
                new Builder<AddTenantDto>()
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.MobileNumber, "9179999999")
                .With(_ => _.CountryCallingCode, "+98")
                .Build();

            string tenantId = await Sut.Add(dto);

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Single().Name
                .Should().Be(dto.Name);
            expected.Single().Email
                .Should().Be(dto.Email);
            expected.Single().Mobile.MobileNumber
                .Should().Be(dto.MobileNumber);
            expected.Single().Mobile.CountryCallingCode
                .Should().Be(dto.CountryCallingCode);
            expected.Single().IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData("dummy@gmail.com")]
        public async Task Add_ptevent_add_tenant_With_dupcaliteEmail(
            string duplicateEmail)
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, duplicateEmail)
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            var dto =
                new Builder<AddTenantDto>()
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, duplicateEmail)
                .With(_ => _.MobileNumber, "9178888888")
                .With(_ => _.CountryCallingCode, "+98")
                .Build();

            var expectedException =
                () => Sut.Add(dto);

            await expectedException.Should()
                .ThrowExactlyAsync<DuplicateEmailException>();

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Single().Name
                .Should().Be(tenant.Name);
            expected.Single().Email
                .Should().Be(tenant.Email);
            expected.Single().Mobile.MobileNumber
                .Should().Be(tenant.Mobile.MobileNumber);
            expected.Single().Mobile.CountryCallingCode
                .Should().Be(tenant.Mobile.CountryCallingCode);
        }

        [Fact]
        public async Task Update_tenant_properly()
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            var dto =
                new Builder<UpdateTenantDto>()
                .With(_ => _.Name, "updatedDummy")
                .With(_ => _.Email, "updatedDummy")
                .With(_ => _.MobileNumber, "9178888888")
                .With(_ => _.CountryCallingCode, "+98")
                .Build();

            await Sut.Update(tenant.Id, dto);

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Single().Name
                .Should().Be(dto.Name);
            expected.Single().Email
                .Should().Be(dto.Email);
            expected.Single().Mobile.MobileNumber
                .Should().Be(dto.MobileNumber);
            expected.Single().Mobile.CountryCallingCode
                .Should().Be(dto.CountryCallingCode);
        }

        [Theory]
        [InlineData("invalidTenantId")]
        public async Task Update_prevent_update_tenant_whenTenantNotExist(
            string invalidTenantId)
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            var dto =
                new Builder<UpdateTenantDto>()
                .With(_ => _.Name, "updatedDummy")
                .Build();

            Func<Task> expectedException =
                () => Sut.Update(invalidTenantId, dto);
            await expectedException.Should()
                .ThrowExactlyAsync<TenantNotExistException>();

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Single().Name
                .Should().Be(tenant.Name);
            expected.Single().Email
                .Should().Be(tenant.Email);
            expected.Single().Mobile.MobileNumber
                .Should().Be(tenant.Mobile.MobileNumber);
            expected.Single().Mobile.CountryCallingCode
                .Should().Be(tenant.Mobile.CountryCallingCode);
        }

        [Theory]
        [InlineData("duplicateEmail")]
        public async Task Update_prevent_update_tenant_whenEmailBeDuplicate(
            string duplicateEmail)
        {
            var tenant1 =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, duplicateEmail)
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            var tenant2 =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            var dto =
                new Builder<UpdateTenantDto>()
                .With(_ => _.Email, duplicateEmail)
                .Build();

            Func<Task> expectedException =
                () => Sut.Update(tenant2.Id, dto);
            await expectedException.Should()
                .ThrowExactlyAsync<DuplicateEmailException>();

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Should().HaveCount(2);
            expected.Single(_ => _.Id == tenant2.Id).Name
                .Should().Be(tenant2.Name);
            expected.Single(_ => _.Id == tenant2.Id).Email
                .Should().Be(tenant2.Email);
            expected.Single(_ => _.Id == tenant2.Id).Mobile.MobileNumber
                .Should().Be(tenant2.Mobile.MobileNumber);
            expected.Single(_ => _.Id == tenant2.Id).Mobile.CountryCallingCode
                .Should().Be(tenant2.Mobile.CountryCallingCode);
        }

        [Fact]
        public async Task Delete_tenant_properly()
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            await Sut.DeleteById(tenant.Id);

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("invalidTenantId")]
        public async Task Delete_prevent_delete_tenant_whenNotExist(
            string invalidTenantId)
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            Func<Task> expectedException =
                () => Sut.DeleteById(invalidTenantId);
            await expectedException.Should()
                .ThrowExactlyAsync<TenantNotExistException>();

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Should().HaveCount(1);
        }

        [Fact]
        public async Task Get_tenant_properly()
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.IsActive, true)
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            var expected = await Sut.GetById(tenant.Id);

            expected.Name
                    .Should().Be(tenant.Name);
            expected.Email
                .Should().Be(tenant.Email);
            expected.MobileNumber
                .Should().Be(tenant.Mobile.MobileNumber);
            expected.CountryCallingCode
                .Should().Be(tenant.Mobile.CountryCallingCode);
            expected.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_tenants_properly()
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.IsActive, true)
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            List<GetTenantDto> expected = await Sut.GetAll();

            expected.Single().Name
                    .Should().Be(tenant.Name);
            expected.Single().Email
                .Should().Be(tenant.Email);
            expected.Single().MobileNumber
                .Should().Be(tenant.Mobile.MobileNumber);
            expected.Single().CountryCallingCode
                .Should().Be(tenant.Mobile.CountryCallingCode);
            expected.Single().IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Update_toggleTenantActivationStatus_properly()
        {
            var tenant =
                await new Builder<Tenant>()
                .With(_ => _.Id, Guid.NewGuid().ToString())
                .With(_ => _.Name, "dummy")
                .With(_ => _.Email, "dummy@gmail.com")
                .With(_ => _.IsActive, true)
                .With(_ => _.Mobile, new Mobile()
                {
                    CountryCallingCode = "+98",
                    MobileNumber = "9179999999"
                })
                .BuildAndSaveInDataBase(Context);

            await Sut.ToggleActivationStatus(tenant.Id);

            var expected = await Context.Set<Tenant>().ToListAsync();
            expected.Single().Name
                    .Should().Be(tenant.Name);
            expected.Single().Email
                .Should().Be(tenant.Email);
            expected.Single().Mobile.MobileNumber
                .Should().Be(tenant.Mobile.MobileNumber);
            expected.Single().Mobile.CountryCallingCode
                .Should().Be(tenant.Mobile.CountryCallingCode);
            expected.Single().IsActive.Should().BeFalse();
        }
    }
}
