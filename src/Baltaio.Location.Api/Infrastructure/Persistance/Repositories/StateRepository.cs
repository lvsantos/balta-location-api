﻿using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Baltaio.Location.Api.Infrastructure.Persistance.Repositories;

public class StateRepository : IStateRepository
{
    private readonly ApplicationDbContext _context;

    public StateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAllAsync(IEnumerable<State> states)
    {
        await _context.States.AddRangeAsync(states);
        await _context.SaveChangesAsync();
    }

    public async Task<List<State>?> GetAllAsync()
    {
        return await _context.States.ToListAsync();
    }

    public async Task<State?> GetAsync(int stateCode, CancellationToken cancellationToken = default)
    {
        State? state = await _context.States.FindAsync(stateCode, cancellationToken);
        return state;
    }
}

