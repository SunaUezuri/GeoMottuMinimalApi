-- Habilita a saída de console para ver o progresso
SET SERVEROUTPUT ON;
-- Desativa a substituição de variáveis (caso haja '&' nos dados)
SET DEFINE OFF;

DECLARE
    -- ### VARIÁVEIS DE CONFIGURAÇÃO ###
    v_total_motos_inserir NUMBER := 10000;
    v_patios_por_filial   NUMBER := 10;
    -- #################################

    -- Variáveis para IDs das Filiais
    v_filial_id_sp        NUMBER;
    v_filial_id_rj        NUMBER;
    v_filial_id_mx_cidade NUMBER;
    v_filial_id_mx_gdl    NUMBER;

    -- Array para armazenar os IDs dos Pátios criados
    TYPE patio_id_array IS TABLE OF NUMBER INDEX BY PLS_INTEGER;
    v_patio_ids           patio_id_array;
    v_patio_counter       PLS_INTEGER := 0;
    
    -- Array para armazenar os IDs das Filiais para facilitar o loop
    TYPE filial_id_array IS TABLE OF NUMBER;
    v_filial_ids          filial_id_array;
    
    -- Variáveis de loop para Motos
    v_patio_alocado       NUMBER;
    v_placa_unica         VARCHAR2(10);
    v_chassi_unico        VARCHAR2(17);
    v_iot_unico           VARCHAR2(50); -- AJUSTADO O TAMANHO
    v_modelo              NVARCHAR2(100);
    v_motor               BINARY_DOUBLE;
    v_pos_x               BINARY_DOUBLE;
    v_pos_y               BINARY_DOUBLE;
    v_modelo_idx          NUMBER;

BEGIN
    DBMS_OUTPUT.PUT_LINE('--- Iniciando carga de dados para GeoMottu ---');

    -- ==============================================
    -- 1. INSERIR FILIAIS
    -- ==============================================
    DBMS_OUTPUT.PUT_LINE('1. Inserindo Filiais...');
    
    INSERT INTO TB_GEOMOTTU_FILIAL (NM_FILIAL, PAIS_FILIAL, ESTADO_FILIAL, ENDERECO_FILIAL, "CriadoEm")
    VALUES ('Mottu - São Paulo', 'Brasil', 'SP', 'Av. Paulista, 1000, São Paulo', SYSTIMESTAMP)
    RETURNING ID_FILIAL INTO v_filial_id_sp;

    INSERT INTO TB_GEOMOTTU_FILIAL (NM_FILIAL, PAIS_FILIAL, ESTADO_FILIAL, ENDERECO_FILIAL, "CriadoEm")
    VALUES ('Mottu - Rio de Janeiro', 'Brasil', 'RJ', 'Av. Rio Branco, 150, Rio de Janeiro', SYSTIMESTAMP)
    RETURNING ID_FILIAL INTO v_filial_id_rj;

    INSERT INTO TB_GEOMOTTU_FILIAL (NM_FILIAL, PAIS_FILIAL, ESTADO_FILIAL, ENDERECO_FILIAL, "CriadoEm")
    VALUES ('Mottu - CDMX', 'Mexico', 'CDMX', 'Paseo de la Reforma, 200, Ciudad de México', SYSTIMESTAMP)
    RETURNING ID_FILIAL INTO v_filial_id_mx_cidade;

    INSERT INTO TB_GEOMOTTU_FILIAL (NM_FILIAL, PAIS_FILIAL, ESTADO_FILIAL, ENDERECO_FILIAL, "CriadoEm")
    VALUES ('Mottu - Guadalajara', 'Mexico', 'Jalisco', 'Av. Vallarta, 3000, Guadalajara', SYSTIMESTAMP)
    RETURNING ID_FILIAL INTO v_filial_id_mx_gdl;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE(SQL%ROWCOUNT || ' filiais inseridas.'); -- (Nota: SQL%ROWCOUNT pode mostrar 1, mas 4 foram inseridas)

    -- ==============================================
    -- 1.5. INSERIR USUÁRIOS (NOVA SEÇÃO)
    -- ==============================================
    DBMS_OUTPUT.PUT_LINE('1.5. Inserindo Usuários...');
    
    -- Usuários para Filial SP
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('Admin SP', 'admin_sp@mottu.com', 'senha123', 'ADMIN', v_filial_id_sp, SYSTIMESTAMP);
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('User SP', 'user_sp@mottu.com', 'senha123', 'USER', v_filial_id_sp, SYSTIMESTAMP);

    -- Usuários para Filial RJ
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('Admin RJ', 'admin_rj@mottu.com', 'senha1ra', 'ADMIN', v_filial_id_rj, SYSTIMESTAMP);
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('User RJ', 'user_rj@mottu.com', 'senha123', 'USER', v_filial_id_rj, SYSTIMESTAMP);
    
    -- Usuários para Filial CDMX
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('Admin CDMX', 'admin_cdmx@mottu.com', 'senha123', 'ADMIN', v_filial_id_mx_cidade, SYSTIMESTAMP);
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('User CDMX', 'user_cdmx@mottu.com', 'senha123', 'USER', v_filial_id_mx_cidade, SYSTIMESTAMP);
    
    -- Usuários para Filial GDL
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('Admin GDL', 'admin_gdl@mottu.com', 'senha123', 'ADMIN', v_filial_id_mx_gdl, SYSTIMESTAMP);
    INSERT INTO TB_GEOMOTTU_USUARIO (NOME_USUARIO, EMAIL_FUNCIONARIO, SENHA_FUNCIONARIO, ROLE_FUNCIONARIO, "FilialId", "CadastradoEm")
    VALUES ('User GDL', 'user_gdl@mottu.com', 'senha123', 'USER', v_filial_id_mx_gdl, SYSTIMESTAMP);

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('8 usuários inseridos.');
    
    -- ==============================================
    -- 2. INSERIR PÁTIOS
    -- ==============================================
    DBMS_OUTPUT.PUT_LINE('2. Inserindo Pátios...');
    v_filial_ids := filial_id_array(v_filial_id_sp, v_filial_id_rj, v_filial_id_mx_cidade, v_filial_id_mx_gdl);

    FOR f IN 1..v_filial_ids.COUNT LOOP -- Loop por cada filial
        FOR i IN 1..v_patios_por_filial LOOP -- Insere N pátios para a filial
            v_patio_counter := v_patio_counter + 1;
            
            INSERT INTO TB_GEOMOTTU_PATIO 
                (CAPC_PATIO, REFERENCIA_PATIO, TAMANHO_PATIO, "TipoDoPatio", "FilialId", "CriadoEm")
            VALUES (
                TRUNC(DBMS_RANDOM.VALUE(50, 201)), 
                'Pátio ' || v_patio_counter || ' - Setor ' || CHR(TRUNC(DBMS_RANDOM.VALUE(65, 68))), 
                DBMS_RANDOM.VALUE(100, 500), 
                CASE TRUNC(DBMS_RANDOM.VALUE(0,2)) WHEN 0 THEN 'Manutencao' ELSE 'Disponivel' END, 
                v_filial_ids(f), 
                SYSTIMESTAMP
            )
            RETURNING ID_PATIO INTO v_patio_ids(v_patio_counter); 
        END LOOP;
    END LOOP;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE(v_patio_counter || ' pátios inseridos.');

    -- ==============================================
    -- 3. INSERIR MOTOS
    -- ==============================================
    DBMS_OUTPUT.PUT_LINE('3. Inserindo ' || v_total_motos_inserir || ' Motos...');
    
    FOR j IN 1..v_total_motos_inserir LOOP
        v_patio_alocado := v_patio_ids(TRUNC(DBMS_RANDOM.VALUE(1, v_patio_ids.COUNT + 1))); 
        
        -- Gera dados únicos
        v_placa_unica := 'MOT' || LPAD(TO_CHAR(j), 7, '0'); 
        v_chassi_unico := 'CH' || LPAD(TO_CHAR(j), 15, '0'); 
        
        -- AJUSTE 1: Código IOT reduzido para 20 caracteres (IOT + 17 números)
        v_iot_unico := 'IOT' || LPAD(TO_CHAR(j), 17, '0');
        
        -- Lógica para Modelo e Motor
        v_modelo_idx := TRUNC(DBMS_RANDOM.VALUE(0, 3)); 
        IF v_modelo_idx = 0 THEN
            v_modelo := 'MottuPop';
            v_motor := 110;
        ELSIF v_modelo_idx = 1 THEN
            v_modelo := 'MottuSport';
            v_motor := 125;
        ELSE
            v_modelo := 'MottuE';
            v_motor := 3000; 
        END IF;

        -- AJUSTE 2: Coordenadas arredondadas para 2 casas decimais
        v_pos_x := ROUND(DBMS_RANDOM.VALUE(-100, -30), 2); -- Longitude
        v_pos_y := ROUND(DBMS_RANDOM.VALUE(-35, 10), 2);   -- Latitude

        INSERT INTO TB_GEOMOTTU_MOTO
            (PLACA_MOTO, CHASSI_MOTO, CD_IOT_PLACA, MOTO_MODELO, MOTOR_MOTO, MOTO_PROPRIETARIO, "PosicaoX", "PosicaoY", "PatioId", "CriadoEm")
        VALUES
            (
                v_placa_unica,
                v_chassi_unico,
                v_iot_unico,
                v_modelo,
                v_motor,
                'Proprietário ' || j, 
                v_pos_x,
                v_pos_y,
                v_patio_alocado,
                SYSTIMESTAMP
            );
            
        IF MOD(j, 1000) = 0 THEN
            COMMIT;
            DBMS_OUTPUT.PUT_LINE('... ' || j || ' motos inseridas.');
        END IF;

    END LOOP;

    COMMIT; 
    DBMS_OUTPUT.PUT_LINE('--- Carga de dados finalizada! Total de ' || v_total_motos_inserir || ' motos inseridas. ---');

EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERRO: ' || SQLERRM);
        ROLLBACK;
END;
/