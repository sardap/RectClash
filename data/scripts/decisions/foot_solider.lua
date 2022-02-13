local STATE_STARTING<const> = "STATE_STARTING"
local STATE_SEARCHING<const> = "STATE_SEARCHING"
local STATE_FOLLOWING<const> = "STATE_FOLLOWING"
local STATE_LEADING<const> = "STATE_LEADING"

local function intialsie_save_table()
    save_table.intialsied = true
    save_table.state = STATE_STARTING
end

local function ident_visible(leader)
    local best_object = nil
    local best = 5000000000
    local min_dist = 5.0
    for i, vision_object in pairs(perception.can_see) do
        local dist = leader:distance(vision_object)
        if dist < best and dist <= min_dist then
            best_object = vision_object
        end
    end

    return best_object
end

local function starting()
    print("starting")
    save_table.state = STATE_SEARCHING
end

local function searching()
    print("searching")

    local leader = get_leader(0)
    if leader == nil then
        return
    end

    if ident_visible(leader) == nil then
        direction_rotate_clockwise()
    else
        save_table.state = STATE_FOLLOWING
        save_table.leader_idx = 0
    end
end

local function following()
    print("following")

    local visible_leader = ident_visible(get_leader(save_table.leader_idx))

    if visible_leader == nil then
        save_table.state = STATE_SEARCHING
    end
end

if save_table.intialsied == nil then
    intialsie_save_table()
end

local c_tbl = {
    STATE_STARTING = starting,
    STATE_SEARCHING = searching,
    STATE_FOLLOWING = following,
    STATE_LEADING = starting
}

local func = c_tbl[save_table.state]
if (func) then
    func()
end

