-- Example
local function create_foot_soliders()
    local decision_script_name<const> = "foot_solider.lua"

    local names = {"lorelai", "amaris", "patricia", "jaelynn", "lilianna", "kaitlin", "elliott", "valery", "lawrence",
                   "samantha", "nehemiah", "john"}

    leader = create_foot_solider_args("andres", decision_script_name)
    leader.hat_rgb = rgb(0, 255, 0)
    leader.body_rgb = rgb(255, 0, 0)
    leader.x = #names / 2.0 * 1.3
    leader.y = 1.0
    leader_id = create_foot_solider(leader)

    for i, name in pairs(names) do
        new_solider = create_foot_solider_args(name, decision_script_name)
        new_solider.x = (1.0 + 0.3) * i
        new_solider.hat_rgb = rgb(255, 182, 193)
        new_solider.body_rgb = rgb(255, 0, 0)
        new_solider.leader = leader_id
        create_foot_solider(new_solider)
    end

end

create_foot_soliders()

